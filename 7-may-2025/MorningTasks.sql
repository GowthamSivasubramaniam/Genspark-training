
--Joins
select au_id as author_id , title from titles join titleauthor on titles.title_id = titleauthor.title_id;
select pub_name publisher , title ,ord_date orderdate  from titles join publishers on titles.pub_id = publishers.pub_id
join sales on titles.title_id = sales.title_id;

select  pub_name publisher,min(ord_date) orderdate from publishers left outer join titles on publishers.pub_id = titles.pub_id
left outer join sales on titles.title_id = sales.title_id
group by pub_name order by 2 desc ;


select title , stor_address as Address from titles as t left outer join sales as s on t.title_id=s.title_id  
left outer join stores as st on s.stor_id = st.stor_id order by 2 desc;

--procedures

create procedure proc_simpleQuery
as
begin
print 'hii'
end

go

proc_simpleQuery

create table Products
( id int identity(1,1) constraint pk_productid primary key ,
name nvarchar(100) not null,
details nvarchar(max) not null
);

create proc proc_InsertProduct(@pname nvarchar(100),@pdetails nvarchar(max))
as
begin
    insert into Products(name,details) values(@pname,@pdetails)
end
go
proc_InsertProduct 'Laptop','{"brand":"Dell","spec":{"ram":"16GB","cpu":"i5"}}'
go
select * from Products

create procedure proc_UpdateProductSpec(@pid int,@newvalue varchar(20))
as
begin
   update products set details = JSON_MODIFY(details, '$.spec.ram',@newvalue) where id = @pid
end

proc_UpdateProductSpec 1, '24GB'

-- JSON queries
select JSON_VALUE(details,'$.brand') PRODUCT_BRAND from products;
select JSON_VALUE(details, '$.spec.ram') Product_Specification from products

 create table Posts
  (id int primary key,
  title nvarchar(100),
  user_id int,
  body nvarchar(max))
Go

  select * from Posts

  create or alter proc proc_BulInsertPosts(@jsondata nvarchar(max))
  as
  begin
insert into Posts(user_id,id,title,body)
 select userId,id,title,body from openjson(@jsondata)
 with (userId int,id int, title varchar(100), body varchar(max))
  end

begin
declare @jsondata nvarchar(max) =
  '[
  {
    "userId": 1,
    "id": 3,
    "title": "sunt aut facere repellat provident occaecati excepturi optio reprehenderit",
    "body": "quia et suscipit\nsuscipit recusandae consequuntur expedita et cum\nreprehenderit molestiae ut ut quas totam\nnostrum rerum est autem sunt rem eveniet architecto"
  }]'
 exec proc_BulInsertPosts @jsondata
 end

 create or alter procedure GetPostsbyUserid (@user_id int)
 as
 begin
 select * from Posts where user_id = @user_id
 end

 GetPostsbyUserid 1