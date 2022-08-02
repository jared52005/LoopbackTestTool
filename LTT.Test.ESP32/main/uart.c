/*******************************************************************************
* @file    UsbIf.c
* @author  Jaromir Mayer
* @brief   Driver for emulation of USB device via UartIf
******************************************************************************
* @attention
******************************************************************************
*/

#include <stdint.h>
#include <stdbool.h>

#include <esp_log.h>

#include "freertos/FreeRTOS.h"
#include "freertos/task.h"
#include "freertos/event_groups.h"

#include "driver/uart.h"
#include "driver/gpio.h"
#include "soc/uart_struct.h"
#include "esp32/rom/uart.h"

#include "uartVcp.h"

/* Private definitions -------------------------------------------------------*/
#define TAG "uart.c"

#define TXD2_PIN (GPIO_NUM_17)
#define RXD2_PIN (GPIO_NUM_16)
#define UART_NUM (UART_NUM_2)
#define TRX_BUF_SIZE 1024

/* Private variables ---------------------------------------------------------*/
static uint8_t trxBuffer[TRX_BUF_SIZE];

/* Private function prototypes -----------------------------------------------*/
void Uart_VcpInit();
void Uart_VcpWrite(uint8_t* DataArray, uint32_t Length);
int Uart_VcpRead(uint8_t* c);

void Uart_VcpInit()
{
    const uart_config_t uart_config = {
        .baud_rate = 115200,
        .data_bits = UART_DATA_8_BITS,
        .parity = UART_PARITY_DISABLE,
        .stop_bits = UART_STOP_BITS_1,
        .flow_ctrl = UART_HW_FLOWCTRL_DISABLE,
		.rx_flow_ctrl_thresh = 0,
        .source_clk = UART_SCLK_APB,
    };
    // We won't use a buffer for sending data.
    uart_driver_install(UART_NUM, TRX_BUF_SIZE, TRX_BUF_SIZE, 0, NULL, 0);
    uart_param_config(UART_NUM, &uart_config);
    uart_set_pin(UART_NUM, TXD2_PIN, RXD2_PIN, UART_PIN_NO_CHANGE, UART_PIN_NO_CHANGE);

	//rxfifo_full_thresh = 1 instead of 120 to get me data when first byte is avaialable
	uart_set_rx_full_threshold(UART_NUM, 1);

	return;
}

void Uart_VcpTask(void *pvParameters)
{
	Uart_VcpInit();
	for(;;)
	{
		int rxBytes = uart_read_bytes(UART_NUM, trxBuffer, TRX_BUF_SIZE, 1000 / portTICK_PERIOD_MS);
		if(rxBytes > 0)
		{
			Uart_VcpWrite(trxBuffer, rxBytes);
		}
	}
}

void Uart_VcpWrite(uint8_t* DataArray, uint32_t Length)
{
	int txBytes = uart_write_bytes(UART_NUM, DataArray, Length);
	ESP_LOGI(TAG, "Uart_VcpWrite() Wrote %d bytes", txBytes);
	if(txBytes != Length)
	{
		ESP_LOGE(TAG, "Uart_VcpWrite() Wrote %d bytes, but was expected to write %d", txBytes, Length);
	}
}
