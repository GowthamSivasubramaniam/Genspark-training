--cursors
-- 1. Write a cursor that loops through all films and prints titles longer than 120 minutes.

begin;

declare cursor_films cursor
for
select * from film where length >120;

fetch all from cursor_films;

close cursor_films;
end;

--2. Create a cursor that iterates through all customers and counts how many rentals each made

begin;

declare cursor_customer_rentals cursor
for
select c.customer_id, concat(first_name,' ',last_name)as CustomerName, count(*) as RentalCount
			from customer c join rental r
			on c.customer_id = r.customer_id
			group by c.customer_id,first_name,last_name;
fetch all from cursor_customer_rentals;
end;


--3. Using a cursor, update rental rates: Increase rental rate by $1 for films with less than 5 rentals
create or replace procedure sp_update_low_rental_rates()
language plpgsql
as $$
declare
    cursor_films cursor for
        select f.film_id, f.rental_rate, count(r.rental_id) as rental_count
        from film f
        left join inventory i on f.film_id = i.film_id
        left join rental r on i.inventory_id = r.inventory_id
        group by f.film_id, f.rental_rate
        having count(r.rental_id) < 5;

    rec record;
begin
    open cursor_films;

    loop
        fetch cursor_films into rec;
        exit when not found;

        update film
        set rental_rate = rec.rental_rate + 1
        where film_id = rec.film_id;

        raise notice 'Updated film_id %: new rate = %', rec.film_id, rec.rental_rate + 1;
    end loop;

    close cursor_films;
end;
$$;

call sp_update_low_rental_rates()

--4. Create a function using a cursor that collects titles of all films from a 
--particular category.

create or replace function movie_by_catid (catid int) 
returns table(title varchar)

language plpgsql
as $$
declare 

 movie_cursor cursor for
 select f.film_id , f.title from film f join film_category c
 on f.film_id = c.film_id where category_id = catid ;
 
 rec record;
 
 begin
 	open movie_cursor;
	 loop
	 fetch movie_cursor into rec;
	  exit when not found;
	 raise notice 'title: % ' , rec.title;
	 return query select rec.title;
	 end loop;
	close movie_cursor;
end;
$$;

select  movie_by_catid (1)

--5. Loop through all stores and count how many distinct 
--films are available in each store using a cursor

create or replace procedure sp_distinct_films()
language plpgsql
as $$
declare
    cursor_films cursor for
        select  store_id, count(distinct film_id) count_of_films  
        from inventory group by store_id;

    rec record;
begin
    open cursor_films;

    loop
        fetch cursor_films into rec;
        exit when not found;
		
        raise notice 'store ID :% , count = %', rec.store_id, rec.count_of_films;
    end loop;

    close cursor_films;
end;
$$;

call sp_distinct_films();


--Triggers
--1.Write a trigger that logs whenever a new customer is inserted.

create table logs (
actions varchar(100),
time date
)
create or replace function notify_new_customer()
returns trigger
language plpgsql
as $$
begin
    insert into logs values('Insert',current_timestamp);
    raise notice 'New Customer added: %', new.customer_id;
    return new;
end;
$$;

create trigger oninsertCustomer after insert on customer
for each row
execute function notify_new_customer();
select * from logs
insert into customer values (
    1523, 1, 'Jared', 'Ely', 'jared.ely@sakilacustomer.org', 530, true, '2006-02-14', '2013-05-26 14:49:45.738'
);


--2. Create a trigger that prevents inserting a payment of amount 0.


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

select * from payment
insert into payment values (
   123445,342,1,1520,2, '2013-05-26 14:49:45.738'
);


--3. Set up a trigger to automatically set last_update on the film table before update.

create or replace function set_lastupadate()
returns trigger
language plpgsql
as $$
begin
    update film set last_update = current_timestamp where
	film_id=new.film_id;
	raise 
	return new;
	
end;
$$;

create trigger onupdateFilm before update on film
for each row
execute function set_lastupadate();


--4. Create a trigger to log changes in the inventory table (insert/delete).

create table inventorylogs (
actions varchar(100),
time date
)
create or replace function notify_inventory_change()
returns trigger
language plpgsql
as $$
begin
    if TG_OP = 'DELETE' then
     insert into inventorylogs values('Delete',current_timestamp);
	else 
	  insert into inventorylogs values('Insert',current_timestamp);
	 end if;
    raise notice 'success';
    return new;
end;
$$;

create trigger oninsertDeleteInventory after insert or delete on inventory
for each row
execute function notify_inventory_change();
select * from inventorylogs
delete from inventory where inventory_id=123456


--5. Write a trigger that ensures a rental can’t be made for a customer who owes more than $50.


select r.customer_id, sum(amount) from rental r join payment p on r.rental_id = p.rental_id 
where return_date is null group by r.customer_id having r.customer_id = 355 ;


create or replace function check_dues()
returns trigger
language plpgsql
as $$
declare
    notreturned_amt int;
begin
    select sum(amount) into notreturned_amt
	from rental r join payment p on r.rental_id = p.rental_id 
    where return_date is null group by r.customer_id having r.customer_id = new.customer_id ;
    if notreturned_amt>=40 then
    raise exception 'customer has overdue of %',notreturned_amt;
	else
	raise notice 'success';
    end if;
	return new;
	
end;
$$;

create trigger oninsertrental before insert on rental
for each row
execute function check_dues();

insert into rental values
(1234568,current_timestamp ,1,359,null,1,current_timestamp);
insert into payment values
(1234568,359,1,123456,50,current_timestamp)

--Transactions
--1. Write a transaction that inserts a customer and an initial rental in one atomic operation.


do $$
declare
    new_customer_id int;
begin
    insert into customer (
        store_id, first_name, last_name, email, address_id, activebool, create_date
    ) values (
        1, 'gowtham', 's', 'gs@example.com', 5, true, current_timestamp
    ) returning customer_id into new_customer_id;

 
    insert into rental (
        rental_date, inventory_id, customer_id, staff_id
    ) values (
        current_timestamp, 1, new_customer_id, 1
    );
end;
$$;

commit;


-- 2. Simulate a failure in a multi-step transaction (update film + insert into inventory) and roll back
begin;

update film
set title = 'New Epic Title'
where film_id = 1;

insert into inventory (film_id, store_id)
values (1, 9999);  

commit;

--3. Create a transaction that transfers an inventory item from one store to another.

begin;

update inventory set store_id = 2 where inventory_id = 100 ;

commit;

--4.Demonstrate SAVEPOINT and ROLLBACK TO SAVEPOINT by updating payment amounts, then undoing one
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

--5. Write a transaction that deletes a customer and all associated rentals and payments, ensuring atomicity.
begin;

delete from payment
where customer_id = 125;


delete from rental
where customer_id = 125;


delete from customer
where customer_id = 125;

commit;

select * from customer where customer_id = 125;