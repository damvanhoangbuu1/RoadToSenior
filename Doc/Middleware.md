# 1. Middleware trong ASP.NET Core
## 1.1. Khái niệm
Request pipeline là một cơ chế xử lý một request đầu vào và kết thúc với đầu ra là một response.

![image](https://github.com/user-attachments/assets/6a9988fc-778d-4ecb-8eca-6a541ce58b45)
-  Request đến từ trình duyệt sẽ đi qua Kestrel web server rồi qua pipeline và quay trở lại khi xử lý xong để trả về client
-  Các thành phần đơn lẻ tạo nên pipeline được gọi là middleware
* Các Middleware là thành phần đóng vai trò tác động vào request pipeline và kết nối lại với nhau thành một xích, middleware đầu tiên nhận HTTP Request, xử lý nó và có thể chuyển cho middleware tiếp theo hoặc trả về ngay HTTP Response.
* **Chuỗi các middleware theo thứ tự như vậy gọi là pipeline**.
## 1.2. Cách hoạt động
Middleware đầu tiên sẽ nhận request, xử lý và gán nó cho middleware tiếp theo.
Middleware cuối cùng sẽ trả request ngược lại cho middleware trước đó.

![image](https://github.com/user-attachments/assets/e63a78a8-ad2b-4a5d-a7f2-c8de3a4508fa)
## 1.3. Vai trò
![image](https://github.com/user-attachments/assets/68f50952-25e8-4a06-827f-802607f97dc7)

Đóng vai trò trung gian giữa request/response và các xử lý logic bên trong web server
Một số vai trò thường gặp
  -  Cần xác thực người dùng để quyết định xem họ có được phép truy cập đến route hiện tại hay không.
  -  Yêu cầu đăng nhập
  -  Chuyển hướng người dùng
  -  Thay đổi/chuẩn hoá các tham số
  -  Xử lý request đầu vào và response được tạo ra,...
## 1.4 Custom và sử dụng Middleware
Muốn sử dụng và custom một middleware cần đảm bảo 2 điều sau:
  -  Class middleware phải khai báo public constructor và với ít nhất một tham số thuộc kiểu RequestDelegate. Đây chính là tham chiếu đến middelware tiếp theo trong pipeline. Khi bạn gọi RequestDelegate này thực tế là bạn đang gọi middleware kế tiếp trong pipeline.
  -  Class middleware phải định nghĩa một method public tên là Invoke nhận một HttpContext và trả về một Task. Đây là phương thức được gọi khi request tới middleware.

Ví dụ:
```c#
  public class CustomeMiddleware
  {
      private readonly RequestDelegate _next;

      public CustomeMiddleware(RequestDelegate next)
      {
          _next = next;
      }

      public async System.Threading.Tasks.Task Invoke(HttpContext context)
      {
          await context.Response.WriteAsync("<div> before - CustomeMiddleware </div>");
          await _next(context);
          await context.Response.WriteAsync("<div> after - CustomeMiddleware </div>");
      }
  }
```
Trong đó class CustomeMiddleware có 1 public constructor có tham só kiểu RequestDelegate là next.
Hàm Ivoke thực hiện các logic( ở đây là `await context.Response.WriteAsync("<div> before - CustomeMiddleware </div>");`) trước khi gọi `await _next(context);` để chuyển đến Middleware tiếp theo. Và `await context.Response.WriteAsync("<div> after - CustomeMiddleware </div>");` là hành động được quy định sau khi Middleware tiếp trả về.

Cuối cùng, chúng ta cần đăng ký middleware trong request pipeline. Chúng ta có thể sử dụng phương thức UseMiddleware trong file Startup.cs như dưới đây:

```c#
app.UseMiddleware<SimpleMiddleware>();
```
