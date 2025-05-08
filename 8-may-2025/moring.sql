use pubs
select * from Products;
-- procedure with out
create procedure getcount(@processor nvarchar(20) , @pcount int out)
as
begin
set @pcount =(select count(*) from products where 
TRY_CAST(json_value(details ,'$.spec.cpu') as nvarchar(20)) = @processor)
end

begin
declare @cnt int
exec getcount 'i5',@cnt out
print @cnt
end

-- Bulk insert with files and exception handling
create table BulkInsertLog
(id int primary key identity(1,1),status nvarchar(20) check(status in ('Success','Failed') ) , message nvarchar(1000));
create table people
( id int primary key ,age int);
begin
Begin try
BULK INSERT people from 'C:\Users\gowtham\Downloads\sample(Sheet1).csv'
with
(Firstrow =2,
fieldterminator =',',
rowterminator = '\n'
)
insert into BulkInsertLog (status,message)
values ('Success','Bulk insert completed');
end try
begin catch
  insert into BulkInsertLog (status,message)
values ('Failed',ERROR_MESSAGE());
end catch
end
select * from people;
select * from BulkInsertLog;
begin
with cte_FailedLogs 
as
(select id , message from BulkInsertLog where status='Failed' )

select * from cte_FailedLogs ;
end

-- pagination
create or alter procedure GetPaginatedBooks (@page int , @pageSize int)
as
begin
with PaginatedBooks as
( select  title_id,title, price, ROW_Number() over (order by price desc) as RowNum
  from titles
)
select * from PaginatedBooks where rowNUm between((@page-1)*(@pageSize+1)) and (@page*@pageSize)
end
GetPaginatedBooks 2,5

-- using offset
select title_id,title, price from titles order by price desc offset 5 rows fetch next 10 rows only
--functions 

create or alter function fn_CalculateTax (@price int , @taxpercent float)
returns float
as
begin
return (@price + (@price * (@taxpercent/100)))
end

select dbo.fn_CalculateTax(100,2)

--table valued function

 create function fn_tableSample(@minprice float)
  returns table
  as
    return select title,price from titles where price>= @minprice

select * from fn_tableSample(10)

--older and slower but supports more logic
create or alter function fn_tableSampleOld(@minprice float)
  returns @Result table(Book_Name nvarchar(100), price float)
  as
  begin
    insert into @Result select title,price from titles where price>= @minprice
	return 
     
end
select * from fn_tableSampleOld(10)



	





