using Data.Entities;
using Microsoft.EntityFrameworkCore;
using Repositories;

namespace AlzaTest.Seed
{
    public static class InitialSeed
    {
        public static async Task EnsureInitialSeed(this IServiceProvider services, CancellationToken cancellationToken = default)
        {
            using var scope = services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            if (await context.Products.AnyAsync(cancellationToken))
            {
                return;
            }

            context.Products.AddRange(
                new Product { Id = 1, Name = "Laptop", Url = "https://alza-products.cz/laptop-url", Price = 999.99m, Description = "Powerful laptop", StockQuantity = 10 },
                new Product { Id = 2, Name = "Mouse", Url = "https://alza-products.cz/mouse-url", Price = 29.99m, Description = "Wireless mouse", StockQuantity = 50 },
                new Product { Id = 3, Name = "Keyboard", Url = "https://alza-products.cz/keyboard-url", Price = 59.99m, Description = "Mechanical keyboard", StockQuantity = 25 },
                new Product { Id = 4, Name = "Webcam", Url = "https://alza-products.cz/webcam-url", Price = 49.99m, Description = "Webcam for streaming", StockQuantity = 5 },
                new Product { Id = 5, Name = "Headphones", Url = "https://alza-products.cz/headphones-anc", Price = 149.99m, Description = "Over-ear headphones with ANC", StockQuantity = 30 },
                new Product { Id = 6, Name = "Monitor 24\"", Url = "https://alza-products.cz/monitor-24-inch-1080p", Price = 179.99m, Description = "24\" 1080p IPS monitor", StockQuantity = 18 },
                new Product { Id = 7, Name = "USB-C Cable 1m", Url = "https://alza-products.cz/usb-c-cable-1m", Price = 9.99m, Description = "USB-C to USB-C 60W cable", StockQuantity = 120 },
                new Product { Id = 8, Name = "Charger 65W", Url = "https://alza-products.cz/charger-65w", Price = 34.99m, Description = "GaN fast charger 65W", StockQuantity = 60 },
                new Product { Id = 9, Name = "Desk Lamp", Url = "https://alza-products.cz/vdesk-lamp-led", Price = 24.99m, Description = "LED desk lamp with dimmer", StockQuantity = 40 },
                new Product { Id = 10, Name = "Bluetooth Speaker", Url = "https://alza-products.cz/bluetooth-speaker", Price = 79.99m, Description = "Portable waterproof speaker", StockQuantity = 22 },
                new Product { Id = 11, Name = "Microphone USB", Url = "https://alza-products.cz/usb-microphone", Price = 89.99m, Description = "Condenser USB microphone", StockQuantity = 16 },
                new Product { Id = 12, Name = "SSD 1TB", Url = "https://alza-products.cz/ssd-1tb", Price = 89.99m, Description = "NVMe SSD 1TB", StockQuantity = 28 },
                new Product { Id = 13, Name = "HDD 2TB", Url = "https://alza-products.cz/hdd-2tb", Price = 64.99m, Description = "3.5\" HDD 2TB", StockQuantity = 35 },
                new Product { Id = 14, Name = "GPU RTX 4070", Url = "https://alza-products.cz/gpu-rtx-4070", Price = 599.00m, Description = "Graphics card RTX 4070", StockQuantity = 8 },
                new Product { Id = 15, Name = "CPU Ryzen 7", Url = "https://alza-products.cz/cpu-ryzen-7", Price = 329.00m, Description = "8-core desktop processor", StockQuantity = 12 },
                new Product { Id = 16, Name = "Motherboard ATX", Url = "https://alza-products.cz/motherboard-atx", Price = 169.00m, Description = "ATX board with Wi-Fi", StockQuantity = 14 },
                new Product { Id = 17, Name = "RAM 32GB (2x16)", Url = "https://alza-products.cz/ram-32gb-3200", Price = 89.00m, Description = "DDR4 32GB 3200MHz kit", StockQuantity = 26 },
                new Product { Id = 18, Name = "PC Case", Url = "https://alza-products.cz/pc-case-atx", Price = 79.00m, Description = "ATX mid-tower case", StockQuantity = 20 },
                new Product { Id = 19, Name = "Power Supply 750W", Url = "https://alza-products.cz/psu-750w-gold", Price = 119.00m, Description = "750W 80+ Gold PSU", StockQuantity = 15 },
                new Product { Id = 20, Name = "Wi-Fi Router AX", Url = "https://alza-products.cz/wifi-router-ax", Price = 139.00m, Description = "Wi-Fi 6 gigabit router", StockQuantity = 19 },
                new Product { Id = 21, Name = "Webcam 4K", Url = "https://alza-products.cz/webcam-4k", Price = 129.00m, Description = "4K autofocus webcam", StockQuantity = 9 },
                new Product { Id = 22, Name = "Gaming Mouse", Url = "https://alza-products.cz/gaming-mouse", Price = 49.00m, Description = "Ergonomic RGB gaming mouse", StockQuantity = 45 },
                new Product { Id = 23, Name = "Mechanical Keyboard Pro", Url = "https://alza-products.cz/mechanical-keyboard-pro", Price = 129.00m, Description = "Hot-swap mechanical keyboard", StockQuantity = 17 },
                new Product { Id = 24, Name = "Portable SSD 2TB", Url = "https://alza-products.cz/portable-ssd-2tb", Price = 159.00m, Description = "USB-C portable SSD 2TB", StockQuantity = 21 },
                new Product { Id = 25, Name = "NVMe Enclosure", Url = "https://alza-products.cz/nvme-enclosure-usbc", Price = 29.00m, Description = "USB-C NVMe enclosure", StockQuantity = 55 },
                new Product { Id = 26, Name = "Thunderbolt Dock", Url = "https://alza-products.cz/thunderbolt-dock", Price = 249.00m, Description = "TB4 dock with power", StockQuantity = 7 },
                new Product { Id = 27, Name = "HDMI Cable 2m", Url = "https://alza-products.cz/hdmi-cable-2m", Price = 12.00m, Description = "UltraHD HDMI 2.1 cable 2m", StockQuantity = 100 },
                new Product { Id = 28, Name = "DisplayPort Cable 2m", Url = "https://alza-products.cz/displayport-cable-2m", Price = 12.00m, Description = "DP 1.4 cable 2m", StockQuantity = 90 },
                new Product { Id = 29, Name = "External HDD 4TB", Url = "https://alza-products.cz/external-hdd-4tb", Price = 109.00m, Description = "USB 3.2 external HDD", StockQuantity = 23 },
                new Product { Id = 30, Name = "Gaming Headset", Url = "https://alza-products.cz/gaming-headset", Price = 79.00m, Description = "Surround gaming headset", StockQuantity = 32 },
                new Product { Id = 31, Name = "Soundbar", Url = "https://alza-products.cz/soundbar", Price = 199.00m, Description = "2.1ch TV soundbar", StockQuantity = 11 },
                new Product { Id = 32, Name = "Smart Plug", Url = "https://alza-products.cz/smart-plug", Price = 19.00m, Description = "Wi-Fi smart plug", StockQuantity = 75 },
                new Product { Id = 33, Name = "USB Hub 7-Port", Url = "https://alza-products.cz/usb-hub-7-port", Price = 24.00m, Description = "Powered USB 3.0 hub", StockQuantity = 50 },
                new Product { Id = 34, Name = "Wireless Charger", Url = "https://alza-products.cz/wireless-charger", Price = 29.00m, Description = "15W Qi wireless charger", StockQuantity = 48 },
                new Product { Id = 35, Name = "Laptop Stand", Url = "https://alza-products.cz/laptop-stand", Price = 34.00m, Description = "Aluminum adjustable stand", StockQuantity = 38 },
                new Product { Id = 36, Name = "Action Camera", Url = "https://alza-products.cz/action-camera-4k", Price = 249.00m, Description = "4K action cam with EIS", StockQuantity = 13 },
                new Product { Id = 37, Name = "Tripod", Url = "https://alza-products.cz/camera-tripod", Price = 39.00m, Description = "Aluminum camera tripod", StockQuantity = 27 },
                new Product { Id = 38, Name = "LED Strip 5m", Url = "https://alza-products.cz/led-strip-5m", Price = 22.00m, Description = "RGB LED strip 5m", StockQuantity = 60 },
                new Product { Id = 39, Name = "SD Card 256GB", Url = "https://alza-products.cz/sd-card-256gb", Price = 29.00m, Description = "UHS-I U3 V30 SD card", StockQuantity = 80 },
                new Product { Id = 40, Name = "Drone", Url = "https://alza-products.cz/camera-drone", Price = 599.00m, Description = "4K camera drone", StockQuantity = 6 }
            );

            await context.SaveChangesAsync(cancellationToken);
        }
    }
}
