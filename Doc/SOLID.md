## 1. Domain Layer
**Mục đích:** Chứa các logic nghiệp vụ cốt lõi và các quy tắc của ứng dụng. Đây là tầng không có phụ thuộc vào bất kỳ thứ gì bên ngoài, giúp giữ cho phần nghiệp vụ được “sạch”.
### Thành phần chính:
**Entities:** Các đối tượng đại diện cho các thực thể trong hệ thống với các thuộc tính và hành vi riêng.
**Value Objects:** Các đối tượng giá trị (thường là các kiểu dữ liệu phức tạp) không thay đổi trạng thái.
**Aggregates:** Các cụm của nhiều entity liên kết chặt chẽ với nhau.
**Repositories (Interfaces):** Định nghĩa các hành vi hoặc truy vấn cần thiết để truy cập dữ liệu.
## 2. Application Layer
**Mục đích:** Định nghĩa các hành vi của hệ thống và cách ứng dụng nên hoạt động. Lớp này chứa logic nghiệp vụ ứng dụng (application business logic) và các dịch vụ để thực hiện các trường hợp sử dụng (use cases) cụ thể.
### Thành phần chính:
**Use Cases (Application Services):** Các dịch vụ hoặc lớp xử lý logic nghiệp vụ ứng dụng, tương ứng với các chức năng hoặc yêu cầu cụ thể của ứng dụng.
**DTOs (Data Transfer Objects):** Các đối tượng trung gian để chuyển dữ liệu giữa các lớp.
**Interfaces:** Định nghĩa các dịch vụ cần thiết từ bên ngoài (ví dụ: gửi email, thanh toán) để có thể thực hiện các use cases.
## 3. Infrastructure Layer
Mục đích: Thực hiện các chi tiết kỹ thuật hoặc cơ sở hạ tầng của hệ thống, bao gồm các logic cần thiết để lưu trữ dữ liệu và truy cập vào các tài nguyên bên ngoài.
### Thành phần chính:
**Implementations of Repositories:** Hiện thực các phương thức truy vấn dữ liệu đã được khai báo trong lớp Domain (ví dụ: với EF Core).
Data Context: Quản lý kết nối tới cơ sở dữ liệu và ánh xạ các đối tượng với cơ sở dữ liệu.
**External Services:** Hiện thực các dịch vụ bên ngoài như email, thông báo, hoặc hệ thống thanh toán.
File Storage: Quản lý lưu trữ tệp tin khi ứng dụng cần lưu hoặc truy cập tệp.
## 4. Presentation Layer (Web API / UI Layer)
**Mục đích:** Là tầng giao tiếp với người dùng hoặc tầng API để truyền tải thông tin và thực thi các thao tác từ bên ngoài.
### Thành phần chính:
**Controllers:** Xử lý các request từ client, gọi các Use Cases (application services), và trả về kết quả.
**View Models:** Cấu trúc dữ liệu để phản hồi và chuyển dữ liệu từ Application Layer tới Presentation Layer.
## Dòng Chảy Phụ Thuộc và Sự Cô Lập
Clean Architecture tuân theo nguyên tắc Dependency Inversion, trong đó các lớp bên trong không phụ thuộc vào các lớp bên ngoài.
Các interface trong Domain và Application layer chỉ định các yêu cầu mà các lớp Infrastructure layer phải đáp ứng. Điều này giúp các lớp bên trong không phụ thuộc vào các chi tiết kỹ thuật của Infrastructure.
## Ưu Điểm của Clean Architecture
Dễ Thay Đổi: Khi thay đổi logic, chúng ta chỉ cần thay đổi ở lớp tương ứng mà không cần chỉnh sửa các phần khác.
Dễ Test: Logic nghiệp vụ dễ dàng được kiểm thử vì các lớp Domain và Application không phụ thuộc vào cơ sở hạ tầng cụ thể.
Tái Sử Dụng: Các lớp Domain và Application có thể tái sử dụng trong các hệ thống khác vì không phụ thuộc vào các chi tiết cụ thể của một ứng dụng nào.