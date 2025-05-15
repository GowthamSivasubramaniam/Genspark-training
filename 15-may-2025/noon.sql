
-- 1. Create a stored procedure to encrypt a given text
-- Task: Write a stored procedure sp_encrypt_text that takes a plain text input (e.g., email or mobile number) and returns an 
-- encrypted version using PostgreSQL's pgcrypto extension.


create or replace procedure sp_encrypt_email1(
    in v_mail text,
    out v_encrypted_base64 text
)
language plpgsql
as $$
declare
    v_encrypted_bytea bytea;
begin
    v_encrypted_bytea := pgp_sym_encrypt(v_mail, 'W9vT8$Xk7^mP@zL2q8#Fs!nR3$dVz&Ae');
    v_encrypted_base64 := encode(v_encrypted_bytea, 'base64');
end;
$$;
call sp_encrypt_email1('sg2@gmail.com', null);



create or replace procedure sp_encrypt_email(
v_mail text,
OUT v_encrypted bytea
)
language plpgsql
as $$
begin
v_encrypted  := pgp_sym_encrypt(v_mail , 'W9vT8$Xk7^mP@zL2q8#Fs!nR3$dVz&Ae');
end;
$$;

call sp_encrypt_email('sg2@gmail.com', null) ;

--2.Create a stored procedure to compare two encrypted texts
--Task: Write a procedure sp_compare_encrypted that takes two 
--encrypted values and checks if they decrypt to the same plain text.


create or replace procedure sp_check_encrypted(
v_mail1 bytea,
v_mail2 bytea
)
language plpgsql
as $$
begin
if pgp_sym_decrypt(v_mail1 , 'W9vT8$Xk7^mP@zL2q8#Fs!nR3$dVz&Ae') =  pgp_sym_decrypt(v_mail2 , 'W9vT8$Xk7^mP@zL2q8#Fs!nR3$dVz&Ae') then
 raise notice 'equals and value is %' , pgp_sym_decrypt(v_mail1 , 'W9vT8$Xk7^mP@zL2q8#Fs!nR3$dVz&Ae');
 else
 raise notice 'Not equals';
 end if;
end;
$$;



DO $$
DECLARE
    v_mail1 BYTEA;
    v_mail2 BYTEA;
BEGIN
    call sp_encrypt_email('sg2@gmail.com', v_mail1) ;
    call sp_encrypt_email('sg2@gmail.com', v_mail2) ;

    CALL sp_check_encrypted(v_mail1, v_mail2);
END;
$$ LANGUAGE plpgsql;


--3. Create a stored procedure to partially mask a given text
--Task: Write a procedure sp_mask_text that:
--Shows only the first 2 and last 2 characters of the input string
--Masks the rest with *

create or replace procedure sp_mask(
v_mail text,
out masked_mail text
)
language plpgsql
as $$
begin
 masked_mail :=  left(v_mail,2)|| '**************' ||right (v_mail,2) as mail;
end;
$$;
call sp_mask('sg12345@gmail.com',null)



--4. Create a procedure to insert into customer with encrypted email and masked name
--Task: Call sp_encrypt_text for email
--Call sp_mask_text for first_name
--Insert masked and encrypted values into the customer table
 
create table customer 
(
 id serial primary key,
 mail text ,
 f_name text
)

create or replace procedure sp_insert_customer(
v_mail text,
v_name text
)
language plpgsql
as $$
declare 
h_mail bytea;
m_name text;
begin
  call sp_encrypt_email(v_mail, h_mail);
  call sp_mask_mail(v_name,m_name);
  insert into customer (mail,f_name)
  values (h_mail,m_name);
  raise notice 'successfully inserted';
end;
$$;

call sp_insert_customer('sg12345@gmail.com','gowtham');
select * from customer;

--5. Create a procedure to fetch and display masked first_name and decrypted email for all customers
--Task:
--Write sp_read_customer_masked() that:
--Loops through all rows
--Decrypts email
--Displays customer_id, masked first name, and decrypted email

create or replace procedure sp_display_customers()
language plpgsql
as $$
declare
    rec record;
    decrypted_mail text;
    masked_name text;
    cur cursor for select mail, f_name from customer;
begin
    open cur;
    loop
        fetch cur into rec;
        exit when not found;

        decrypted_mail := pgp_sym_decrypt(rec.mail::bytea, 'W9vT8$Xk7^mP@zL2q8#Fs!nR3$dVz&Ae');

        masked_name := rec.f_name;

        raise notice 'decrypted mail: %, masked name: %', decrypted_mail, masked_name;
    end loop;
    close cur;
end;
$$;


call sp_display_customers()