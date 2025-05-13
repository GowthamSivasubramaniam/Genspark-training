--1. Write a cursor to list all customers and 
--how many rentals each made. Insert these into a summary table.
select * from customer
select * from rental
create table summary 
(id serial primary key ,
  customer_id int ,
  rental_count int)

do $$
declare
    rec record;
    cur cursor for
	select c. customer_id , count(rental_id) as count 
	from customer c join rental r on c.customer_id = r.customer_id
	group by c.customer_id;
begin 
 open cur;
   loop
     fetch cur into rec;
	 exit when not found;

	 insert into summary (customer_id , 
     rental_count)
	 values(rec.customer_id,rec.count);
	end loop;
end;
$$;

select * from summary;

--2 .Using a cursor, print the titles of films in the 'Comedy' 
--  category rented more than 10 times.

select * from inventory;
select * from rental;

do $$
declare
    rec record;
	rental_count int;
    cur_1 cursor for
	select f.film_id , title 
	from film f join  film_category c on c.film_id = f.film_id 
	join category cat on cat.category_id = c.category_id 
	where name = 'Comedy';
begin 
 open cur_1;
   loop
     fetch cur_1 into rec;
	 exit when not found;
     select count(*) into rental_count from inventory i
	 join rental r on i.inventory_id = r.inventory_id
	 where rec.film_id = i.film_id ;

	 raise notice 'movie: % , rents: %' ,rec.title ,rental_count;
	end loop;
end;
$$;

-- 3. Create a cursor to go through each store and count the 
--number of distinct films available, and insert results into a report table.
create table report_table 
(id serial primary key ,
  store_id int ,
  distinct_film_count int)
select * from report_table;
do $$
declare
    rec record;
    cur cursor for
	select store_id , count(distinct(film_id)) as count 
	from inventory group by store_id;
begin 
 open cur;
   loop
     fetch cur into rec;
	 exit when not found;

	 insert into report_table (store_id , 
     distinct_film_count)
	 values(rec.store_id,rec.count);
	end loop;
end;
$$;

--Loop through all customers who haven't rented in the last 6 months and 
--insert their details into an inactive_customers table.

create table inactive_customers
(
 id serial primary key,
 name text,
 customer_id int
);

select * from inactive_customers
do $$
declare
    rec record;
    cur cursor for
	select (first_name ,' ',last_name ) as name ,c.customer_id
	from rental r join customer c on r.customer_id = c.customer_id
	where rental_date < current_timestamp::timestamp - INTERVAL '6 months' order by 
	rental_date desc , c.customer_id ;
	
begin 
 open cur;
   loop
     fetch cur into rec;
	 exit when not found;

	 insert into inactive_customers (name , 
     customer_id)
	 values(rec.name,rec.customer_id);
	end loop;
end;
$$;


--1.Write a transaction that inserts a new customer, 
--adds their rental, and logs the payment – all atomically.
select * from customer;

DO $$ 
DECLARE
    v_customer_id INT;
    v_rental_id INT;
    v_staff_id INT;
	
BEGIN

insert into customer values
(600 ,1 ,'gowtham' , 'sivasubramainam','sg@gmail.com',3,true,current_timestamp,
current_timestamp,1) returning customer_id INTO v_customer_id;

insert into rental
values(10989006,current_timestamp, 1,v_customer_id,current_timestamp::timestamp + INTERVAL '10 days',1,
current_timestamp) returning rental_id ,staff_id INTO v_rental_id,v_staff_id;

insert into payment 
 values(123456,v_customer_id,v_staff_id,v_rental_id,6.99,current_timestamp);

END $$;


--2. Simulate a transaction where one update fails (e.g., invalid rental ID),
-- and ensure the entire transaction rolls back.

do $$

BEGIN
insert into customer values
(601 ,1 ,'gowtham' , 'sivasubramainam','sgs@gmail.com',3,true,current_timestamp,
current_timestamp,1);
insert into customer values
(600 ,1 ,'gowtham' , 'sivasubramainam','sg@gmail.com',3,true,current_timestamp,
current_timestamp,1);
commit;
exception
 	WHEN OTHERS THEN
        raise notice '%',SQLERRM
	ROLLBACK;
end $$;

-- 3. Use SAVEPOINT to update multiple payment amounts. 
--Roll back only one payment update using ROLLBACK TO SAVEPOINT.

begin;

update payment
set amount = amount + 20
where payment_id = 17503;

savepoint before_second_update;

update payment
set amount = amount + 20
where payment_id = 17504;

rollback to savepoint before_second_update;

commit;
select * from payment where payment_id = 17503;
select amount from payment where payment_id = 17504;

--4.Perform a transaction that transfers inventory from one store 
--to another (delete + insert) safely.
start transaction;

alter table  rental
drop constraint rental_inventory_id_fkey;

delete from inventory where inventory_id = 100 ;

insert into inventory values
(100,20,2,current_timestamp);

alter table rental
ADD constraint rental_inventory_id_fkey
foreign key (inventory_id)
references inventory(inventory_id);

commit;

-- 5.Create a transaction that deletes a customer and all associated 
--records (rental, payment), ensuring referential integrity.

begin;

delete from payment
where customer_id = 125;


delete from rental
where customer_id = 125;


delete from customer
where customer_id = 125;

commit;

select * from customer where customer_id = 125;


--1. Create a trigger to prevent inserting payments of zero or negative amount.
create or replace function check_amt()
returns trigger
language plpgsql
as $$
begin
    if new.amount<=0 then
    raise exception 'amount must be positive';
	else
	raise notice 'success';
    end if;
	return new;
	
end;
$$;

create trigger oninsertpayment before insert on payment
for each row
execute function check_amt();

select * from payment where payment_id = 12835;
insert into payment values (
   12835,342,1,1520,-2, '2013-05-26 14:49:45.738'
);


--2.Set up a trigger that automatically updates 
--last_update on the film table when the title or rental rate is changed
create or replace function func_set_last_update()
returns trigger
language plpgsql
as $$
begin
update film set last_update = current_timestamp where film_id =new.film_id;
raise notice 'last update : %',current_timestamp;
return new;
end;
$$;

create or replace trigger trig_update_film_date after update on film
for each row
WHEN (
  OLD.title IS DISTINCT FROM NEW.title OR
  OLD.rental_rate IS DISTINCT FROM NEW.rental_rate
)
execute function func_set_last_update();

select * from film where film_id =1;
update film set title = 'Salaar' where film_id =1;
update film set rating = 'PG-13' where film_id =1;




--3.Write a trigger that inserts a log into 
-- rental_log whenever a film is rented more than 3 times in a week.

create table rental_log
(
film_id int,
title text,
week_starting_date date

)


create or replace function func_rental_log()
returns trigger
language plpgsql
as $$
declare 
 id int;
 rental_count int; 
 rent_title text;
begin

 select film_id , count(*)into id,rental_count from inventory i join rental r on r.inventory_id = i.inventory_id
 where date_trunc('week',new.rental_date) =  date_trunc('week',rental_date)
 group by r.inventory_id,film_id having r.inventory_id = new.inventory_id;

 select title into rent_title from film f where
 f.film_id = id;
 if rental_count >= 3 then
  INSERT INTO rental_log 
  VALUES (id, rent_title, date_trunc('week',new.rental_date));
 end if;
  return new;
  end;
 $$;

create or replace trigger trig_update_rental_log after insert on rental
for each row
execute function func_rental_log();



select * from rental_log;
INSERT INTO rental (rental_id, rental_date, inventory_id, customer_id, return_date, staff_id, last_update)
VALUES (20007, CURRENT_TIMESTAMP, 102, 1, CURRENT_TIMESTAMP + INTERVAL '3 days', 1, CURRENT_TIMESTAMP);

INSERT INTO rental (rental_id, rental_date, inventory_id, customer_id, return_date, staff_id, last_update)
VALUES (20008, CURRENT_TIMESTAMP, 102, 2, CURRENT_TIMESTAMP + INTERVAL '8 days', 1, CURRENT_TIMESTAMP + INTERVAL '5 days');

INSERT INTO rental (rental_id, rental_date, inventory_id, customer_id, return_date, staff_id, last_update)
VALUES (20009, CURRENT_TIMESTAMP, 102, 3, CURRENT_TIMESTAMP + INTERVAL '13 days', 1, CURRENT_TIMESTAMP + INTERVAL '10 days');
INSERT INTO rental (rental_id, rental_date, inventory_id, customer_id, return_date, staff_id, last_update)
VALUES (20011, CURRENT_TIMESTAMP, 102, 3, CURRENT_TIMESTAMP + INTERVAL '13 days', 1, CURRENT_TIMESTAMP + INTERVAL '10 days');

 

 



