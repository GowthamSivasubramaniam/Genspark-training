--cursor hands on

declare pub_cursor cursor for
select pub_id , pub_name from publishers

open pub_cursor
	declare @pub_id int , @pubname nvarchar(max)
	fetch next from pub_cursor into @pub_id , @pubname
	while @@FETCH_STATUS =0
	begin
		declare book_cursor cursor for
		select title from titles where pub_id = @pub_id
		print concat('publisher :' , @pubname)
		declare @book nvarchar(max)
		declare @i int = 1 
		open book_cursor
		fetch next from book_cursor into @book
		if  @@FETCH_STATUS <> 0
		print '  No books have been published'
		while @@FETCH_STATUS =0
		begin
		print concat('  ',@i,'.  ', @book)
		SET @i=@i+1
		fetch next from book_cursor into @book
		end
		close book_cursor
		deallocate book_cursor
	fetch next from pub_cursor into @pub_id , @pubname
	end
	close pub_cursor


 -- 1) List all orders with the customer name and the employee who handled the order.
--(Join Orders, Customers, and Employees)

select OrderID , CompanyName as Customer , concat(FirstName,' ',LastName) Employee_Name from orders o
join Customers c on o.CustomerID = c.CustomerID 
join Employees e on o.EmployeeID = e.EmployeeID

--2) Get a list of products along with their category and supplier name.
--(Join Products, Categories, and Suppliers)

select * from products
select ProductName ,CategoryName, CompanyName from products p
join Categories c on p.CategoryID = c.CategoryID 
join Suppliers s on s.SupplierID =p.SupplierID

--3) Show all orders and the products included in each order with quantity and unit price.
--(Join Orders, Order Details, Products)
select * from [Order Details]
select ProductName,o.*,od.UnitPrice,Quantity,Discount from [Order Details] od
join Products p on p.ProductID= od.ProductID 
join Orders o on o.OrderID = od.OrderID

--4) List employees who report to other employees (manager-subordinate relationship).
--(Self join on Employees)

select * from Employees;
select concat(e.FirstName,' ',e.LastName) as Employee , concat(m.FirstName,' ',m.LastName) as Manager  from Employees e
join Employees m on e.ReportsTo =m.EmployeeID;

--5) Display each customer and their total order count.
--(Join Customers and Orders, then GROUP BY)

select CompanyName,count(o.CustomerID) order_count from Customers c join Orders o on c.CustomerID = o.CustomerID 
group by o.CustomerID,CompanyName;

--6) Find the average unit price of products per category.
select CategoryName ,avg(UnitPrice) as AvgUnitPrice from Categories c join Products p 
on c.CategoryID = p.CategoryID group by p.CategoryID,CategoryName

--7) List customers where the contact title starts with 'Owner'.
select * from Customers where ContactTitle Like 'Owner%';

--8) Show the top 5 most expensive products.
select top 5 * from Products Order by UnitPrice Desc 

--9) Return the total sales amount (quantity × unit price) per order.
select OrderID, sum(UnitPrice * Quantity) as Total_price from [Order Details] group by OrderID

--10)  Create a stored procedure that returns all orders for a given customer ID.
create or alter procedure OrdersByCustomerId (@cusId nvarchar(max))
as
begin
select * from Orders o join [Order Details] od 
on  o.OrderId = od .OrderID
where CustomerID =@cusId 
end

OrdersByCustomerId 'ANATR'

--11) Write a stored procedure that inserts a new product.

select * from Products

create procedure AddProduct(@name nvarchar(max),@sid int,@cid int ,@qpu nvarchar(max),@up float,@uis int,@uoo int, @rl int,
@Dis int )
as
begin
insert into Products values(@name,@sid,@cid,@qpu,@up,@uis,@uoo,@rl,@Dis)
end

AddProduct 'Coffee',1,1,'20 boxes',25.00,20,0,10,0


select * from Employees

--12) Create a stored procedure that returns total sales per employee.

create or alter procedure getTotalSalesPerEmployee (@eid int )
as
begin
select o.EmployeeID,sum(od.UnitPrice * od.Quantity) as Total_sales from Orders o join [Order Details] od 
on  o.OrderId = od .OrderID
join Employees e on o.EmployeeID = e.EmployeeID
group by o.employeeID having o.EmployeeID = @eid
end

getTotalSalesPerEmployee 2

--13) Use a CTE to rank products by unit price within each category.

with cte_rankProducts
as
(
select *,RANK() OVER (PARTITION BY CategoryID ORDER BY ProductID DESC) AS ProductRank from Products
)
select * from cte_rankProducts

with cte_totalRevenue
as
(
select *,RANK() OVER (PARTITION BY CategoryID ORDER BY ProductID DESC) AS ProductRank from Products
)
select * from cte_rankProducts

--14) total revenue by product

with cte_products
as
(select o.ProductID,ProductName,sum(o.unitPrice * Quantity) Revenue from  [Order Details] o join Products p on o.ProductID = p.ProductID
group by o.ProductID,ProductName)
select * from cte_products where ProductID = 2

--15) Use a CTE with recursion to display employee hierarchy.

--Start from top-level employee (ReportsTo IS NULL) and drill down

WITH EmployeeHierarchy AS (
    -- Anchor member: Top-level employees (e.g. no manager)
    SELECT 
        EmployeeID,
        concat(FirstName,' ' ,LastName) EmployeeName,
        ReportsTo,
        0 AS Level
    FROM Employees
    WHERE ReportsTo IS NULL

    UNION ALL

    SELECT 
        e.EmployeeID,
        concat(e.FirstName,' ' ,e.LastName) EmployeeName,
        e.ReportsTo,
        eh.Level + 1
    FROM Employees e
    INNER JOIN EmployeeHierarchy eh ON e.ReportsTo = eh.EmployeeID
)
SELECT * 
FROM EmployeeHierarchy
ORDER BY Level, ReportsTo;
