
--1. Try two concurrent updates to same row → see lock in action.

-- Transaction A
start transaction ;
update tbl_bank_accounts set balance = balance+1000 where account_id =1 ;
commit;

-- Transaction B
start transaction ;
update tbl_bank_accounts set balance = balance+1000 where account_id =1 ;
commit;

-- Answer : Until one update transaction finishes the other one could not be done
-- eg: A starts and updates only After it commits , then B can  update , 
-- in this B can Read But cant write  so it is more like Exclusive lock.
-- id -36898 locktype -"transactionid"  mode-"ShareLock" granted-false


--2 .Write a query using SELECT...FOR UPDATE and check how it locks row.

-- Transaction A
start transaction ;
select * from tbl_bank_accounts where account_id =1 for update;
commit;

-- Transaction B
start transaction ;
update tbl_bank_accounts set balance = balance+1000 where account_id =1 ;
commit;

--Transaction C
start transaction ;
update tbl_bank_accounts set balance = balance+1000 where account_id =2 ;
commit;

--Answer : if update is done on a row that is locked using "row lock" then
-- that update will be stalled untill the transaction that has acquired that 
-- row lock ends. example use above queries
-- A starts first and B starts after A but A didn't complete and c starts 
-- c will be allowed to commit since row lock is on rows where id = 1 so b should wait
--until A commits.


--3. Intentionally create a deadlock and observe PostgreSQL cancel one transaction.
-- Transaction A
start transaction ;
update tbl_bank_accounts set balance = balance+1000 where account_id =2 ;
update tbl_bank_accounts set balance = balance+1000 where account_id =1 ;
commit;

-- Transaction B
start transaction ;
update tbl_bank_accounts set balance = balance+1000 where account_id =1 ;
update tbl_bank_accounts set balance = balance+1000 where account_id =2 ;
commit;

--Answer : The transaction that starts first is completed and the other is aborted by the 
--postgre.
--ERROR:  deadlock detected
--Process 40783 waits for ShareLock on transaction 1045; blocked by process 36898.
--Process 36898 waits for ShareLock on transaction 1047; blocked by process 40783. 

--SQL state: 40P01
--Detail: Process 40783 waits for ShareLock on transaction 1045; blocked by process 36898.
--Process 36898 waits for ShareLock on transaction 1047; blocked by process 40783.
--Context: while updating tuple (0,9) in relation "tbl_bank_accounts"


--4 .Use pg_locks query to monitor active locks.

select
    a.pid,
    a.usename,
    a.state,
    a.query as current_query,
    c.relname as locked_table,
    l.locktype,
    l.mode,
    l.granted,
    l.transactionid,
    l.virtualxid,
    l.virtualtransaction
from pg_locks l
left join pg_stat_activity a on l.pid = a.pid
left join pg_class c on l.relation = c.oid
where not l.granted or a.state != 'idle'
order by l.pid;

-- the above query will display all the info of locks that are not idle;

--5.Explore about Lock Modes.

--Access share
-- transaction A
begin;
-- transaction A acquires an access share lock
select * from tbl_bank_accounts where account_id = 1;

-- transaction B
begin;
-- transaction B also acquires an access share lock (compatible)
select * from tbl_bank_accounts where account_id = 1;

------
-- Row share lock
-- transaction A
begin;
-- transaction A acquires a row share lock
select * from tbl_bank_accounts where account_id = 1 for update;

-- transaction B
begin;
lock table tbl_bank_accounts in exclusive mode;
-- transaction B tries to acquire a conflicting row share lock and also get blocked 
--if try to update rows with account_id = 1
-- will block if using exclusive lock or DDL 

------

-- Exclusive lock
-- transaction A
begin;
-- transaction A acquires an exclusive lock
lock table tbl_bank_accounts in exclusive mode;

-- transaction B
begin;
update tbl_bank_accounts set balance = 50000 where account_id = 1;
-- transaction B tries to update a row
-- will block until transaction A commits


--Access exclusive lock

-- transaction A
begin;
lock table tbl_bank_accounts in access exclusive mode;
-- transaction A acquires an access exclusive lock
alter table tbl_bank_accounts add column last_updated timestamp;

-- transaction B
begin;
-- transaction B tries to select or update the table
-- will block until transaction A commits
select * from tbl_bank_accounts where account_id = 1;
-- or
update tbl_bank_accounts set balance = 60000 where account_id = 1;