# 🚦 TrafficLight-Arduino

Hệ thống mô phỏng **đèn giao thông tại giao lộ** sử dụng Arduino, kết hợp giao diện điều khiển bằng **Windows Forms (C#)**.

## 📋 Giới thiệu

Dự án mô phỏng một giao lộ hai chiều với đầy đủ các chế độ hoạt động của đèn giao thông. Hệ thống được thiết kế theo **hướng đối tượng (OOP)** trên Arduino, dễ mở rộng và bảo trì.

**Tính năng chính:**
- Điều khiển thời gian đèn xanh, đỏ, vàng cho từng hướng
- Chế độ **Bình thường** (tuần tự)
- Chế độ **Dừng khẩn cấp** (điều khiển thủ công từng đèn)
- Chế độ **Đèn vàng nhấp nháy** (có hẹn giờ theo ngày/giờ)
- Chế độ **Ban đêm** tự động (từ 22h đến 5h sáng)
- Hiển thị thời gian đếm ngược trên **4 cụm LED 7 đoạn**
- Điều khiển từ xa qua **Bluetooth (HC-05)** hoặc Serial
- Giao diện Windows Forms hiện đại để cấu hình dễ dàng
- Lưu lịch sử hoạt động vào **SQL Server**

## 🛠 Công nghệ sử dụng

### Phần cứng & Arduino
- **Arduino** (khuyến nghị Uno/Nano/Mega)
- LED 7 đoạn 8 chữ số (74HC595)
- 6 LED giao thông (2 bộ 3 màu)
- Module RTC DS1307 (đồng hồ thời gian thực)
- Module Bluetooth HC-05
- TimerOne library

### Phần mềm
- **C++** (Arduino) - OOP
- **C# Windows Forms** (.NET Framework)
- SQL Server (lưu lịch sử)

## 📁 Cấu trúc dự án
TrafficLight-Arduino/
├── Arduino/                  # Code Arduino
│   ├── Arduino.ino
│   ├── TrafficLight.h/cpp
│   ├── GiaoLo.h/cpp
│   ├── Led7Seg.h/cpp
│   └── ...
├── TrafficLight/             # Ứng dụng Windows Forms
│   ├── TrafficLight.sln
│   └── TrafficLight/
│       ├── Form1.cs          # Giao diện chính
│       ├── Login.cs
│       └── Database.cs
└── README.md
text## ✨ Tính năng nổi bật

- **Code Arduino hướng đối tượng**: Sử dụng class `TrafficLight`, `GiaoLo`, `Led7Seg`
- **Điều khiển linh hoạt**: Có thể thay đổi thời gian đèn theo từng hướng
- **Hỗ trợ Proteus**: Dễ dàng mô phỏng mà không cần phần cứng thật
- **Giao diện thân thiện**: Dễ dàng thao tác cho người dùng
- **Lưu lịch sử**: Theo dõi các lần thay đổi chế độ

## 🚀 Hướng dẫn sử dụng

### 1. Phần Arduino
1. Mở `Arduino/Arduino.ino`
2. Cài đặt các thư viện: `TimerOne`, `RTClib`
3. Nạp code vào board Arduino
4. Kết nối với Proteus hoặc phần cứng thật

### 2. Phần Windows Forms
1. Mở solution `TrafficLight.sln`
2. Chạy project
3. Chọn cổng COM tương ứng với Arduino/Bluetooth
4. Sử dụng giao diện để điều khiển

## 📷 Hình ảnh dự án


## 🔧 Tác giả

**HoangQuyCoder**

## 📄 License

Dự án này được phân phối dưới giấy phép MIT.
