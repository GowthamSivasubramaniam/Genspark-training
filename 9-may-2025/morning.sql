--  List all films with their length and rental rate, sorted by length descending.
select title,rental_rate,length from film order by 3 desc;

--  Find the top 5 customers who have rented the most films.

select r.customer_id,count(*) from rental r join customer c on r.customer_id = c.customer_id group by 
r.customer_id order by 2 desc limit 5

--Display all films that have never been rented.

select title from film f 
left join  inventory i on f.film_id = i.film_id  
left join rental r on r.inventory_id = i.inventory_id 
where rental_id is null

--List all actors who appeared in the film ‘Academy Dinosaur’.

select concat(first_name,' ',last_name),title from actor a 
join film_actor fa on a.actor_id = fa.actor_id
join film f on fa.film_id=f.film_id
where title = 'Academy Dinosaur'

--List each customer along with the total number of rentals they made and the total amount paid.

select  r.customer_id, concat(first_name,' ',last_name) , count(r.rental_id) as rents , sum(amount) amount from customer c join rental r 
on c.customer_id = r.customer_id join payment p on r.rental_id = p.rental_id group by
r.customer_id,2 order by 3 desc , 4 desc

--Using a CTE, show the top 3 rented movies by number of rentals.
--Columns: title, rental_count

with cte_topRentedFilms
as (
select f.film_id,r.rental_id,title from
film f join  inventory i on f.film_id = i.film_id  
join rental r on r.inventory_id = i.inventory_id 
)
select title , count(*) from cte_topRentedFilms group by film_id,title order by 2 desc limit 3

--Find customers who have rented more than the average number of films.
--Use a CTE to compute the average rentals per customer, then filter.

with cte_avg_rentals as (
    select avg(rental_count) as avg_rentals
    from (
        select customer_id, count(rental_id) as rental_count
        from rental
        group by customer_id
    ) sub
)
select 
    r.customer_id, 
    count(r.rental_id) as customer_rentals
from rental r
join customer c on r.customer_id = c.customer_id
group by r.customer_id
having count(r.rental_id) > (select avg_rentals from cte_avg_rentals);

--Write a function that returns the total number of rentals for a given customer ID.
--Function: get_total_rentals(customer_id INT)


create or replace function getrentals(cus_id int)
returns int
as $$
declare
    a int;
begin
    select count(*) into a
    from rental
    where customer_id = cus_id;

    return a;
end;
$$ language plpgsql;

select getrentals(1);
select * from film

--Write a stored procedure that updates the rental rate of a film by film ID and new rate.
--Procedure: update_rental_rate(film_id INT, new_rate NUMERIC)

create or replace procedure sp_updateRentalRate (id int , rate Numeric)
language plpgsql
as $$
begin
update film set rental_rate = rate where film_id =id;
end;
$$;
call sp_updateRentalRate(1,6)
select film_id , rental_rate from film order by 1

--Write a procedure to list overdue rentals (return date is NULL and rental date older than 7 days).
--Procedure: get_overdue_rentals() that selects relevant columns.

create or replace procedure sp_overdue_rentals()
language plpgsql
as $$
declare
    r record;
begin
for r in 
	select rental_id, customer_id, rental_date, return_date
	from rental where return_date is null or date(return_date) - date(rental_date) > 7
loop
  raise notice 'Rental ID: %, Customer ID: %, Rental Date: %, Return Date: %', 
            r.rental_id, r.customer_id, r.rental_date, r.return_date;
end loop;
end;
$$;
call sp_overdue_rentals()
select film_id , rental_rate from film order by 1
