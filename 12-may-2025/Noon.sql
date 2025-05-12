--1. In a transaction, if I perform multiple updates and an error happens in the third statement, but I have not used SAVEPOINT, what will happen if I issue a ROLLBACK?
--Will my first two updates persist?

start transaction ;

update tbl_bank_accounts set balance = balance+1000 where account_id =1 ;
select * from tbl_bank_accounts where account_id =1;

update tbl_bank_accounts set balance = balance+1000 where account_id =1 ;
select * from tbl_bank_accounts where account_id =1;

update tbl_bank_account set balance = balance+1000 where account_id =1 ;
select * from tbl_bank_accounts where account_id =1;

Rollback;
select * from tbl_bank_accounts;

-- Answer : all the updates that are part of the current transaction will be aborted 
--i.e  the changes won't persists


-- 2. Suppose Transaction A updates Alice’s balance but does not commit. 
-- Can Transaction B read the new balance if the isolation level is set to READ COMMITTED?

--Transation A
start transaction ;
update tbl_bank_account set balance = balance+1000 where account_id =1 ;

--Transation B
start transaction isolation level Read Committed;
select balance from tbl_bank_accounts where account_id =1;

--Answer : It won't update the new balance until it is committed.



--3.What will happen if two concurrent transactions both execute:
--UPDATE tbl_bank_accounts SET balance = balance - 100 WHERE account_name = 'Alice';
--at the same time? Will one overwrite the other?

--Transation A
start transaction ;
UPDATE tbl_bank_accounts SET balance = balance - 100 WHERE account_name = 'Alice';

--Transation B
start transaction isolation level Read Committed;
UPDATE tbl_bank_accounts SET balance = balance - 100 WHERE account_name = 'Alice';


--Answer : It wont allow until one transaction is commited , example
--if transaction 'A' updates first and still not committed , now 
--transaction 'B' updates , the updates of transaction 'B' is queued until
-- A commits , and once the A commits , B's update will be allowed. 

--4. If I issue ROLLBACK TO SAVEPOINT after_alice;, will it only undo changes 
--made after the savepoint or everything?

start transaction ;

update tbl_bank_accounts set balance = balance+1000 where account_id =1 ;
select balance from tbl_bank_accounts where account_id =1; 

savepoint "after_alice";

update tbl_bank_accounts set balance = balance+1000 where account_id =1 ;
select balance from tbl_bank_accounts where account_id =1;

rollback to "after_alice";
commit;
--Answer : yes,It undo only the changes made after the savepoint.

--5.Which isolation level in PostgreSQL prevents phantom reads?

start transaction isolation level repeatable read;
select balance from tbl_bank_accounts where balance> 1000; 
commit;

-- Answer : serializable because it uses snapshots . 

--6. Can Postgres perform a dirty read 
--(reading uncommitted data from another transaction)?

--  Answer : No it is not allowed in Postgres.

--7. If autocommit is ON (default in Postgres), 
--and I execute an UPDATE, is it safe to assume the change is immediately committed?

-- yes , if no other transaction that uses 
-- "start transaction"  command initially for updating the same record ;
-- example 

start transaction ;
update tbl_bank_accounts set balance = balance+1000 where account_id =1 ;
-- if , it is there no auto commit will work until this transaction got committed

--8.
--If done this:
BEGIN;
UPDATE tbl_bank_accounts SET balance = balance - 500 WHERE account_id = 1;
-- (No COMMIT yet)
--And from another session, I run:
commit;
SELECT balance FROM tbl_bank_accounts WHERE account_id = 1;
--Will the second session see the deducted balance?

--answer : No , until first commits , the second cant see the detection