Primary dbserver commands:

---------------------------------------------------------------------------------------------

command : initdb -D "C:/pri"


The files belonging to this database system will be owned by user "gowtham".
This user must also own the server process.
The database cluster will be initialized with locale "English_India.1252".
The default database encoding has accordingly been set to "WIN1252".
The default text search configuration will be set to "english".
Data page checksums are disabled.
creating directory C:/pri ... ok
creating subdirectories ... ok
selecting dynamic shared memory implementation ... windows
selecting default "max_connections" ... 100
selecting default "shared_buffers" ... 128MB
selecting default time zone ... Asia/Calcutta
creating configuration files ... ok
running bootstrap script ... ok
performing post-bootstrap initialization ... ok
syncing data to disk ... ok

initdb: warning: enabling "trust" authentication for local connections
initdb: hint: You can change this by editing pg_hba.conf or using the option -A, or --auth-local and --auth-host, the next time you run initdb.

Success. You can now start the database server using:

    ^"C^:^\Program^ Files^\PostgreSQL^\17^\bin^\pg^_ctl^" -D C:/pri -l logfile start


-------------------------------------------------------------------------------------------

command : initdb -D "C:/sec"


The files belonging to this database system will be owned by user "gowtham".
This user must also own the server process.
The database cluster will be initialized with locale "English_India.1252".
The default database encoding has accordingly been set to "WIN1252".
The default text search configuration will be set to "english".
Data page checksums are disabled.
creating directory C:/sec ... ok
creating subdirectories ... ok
selecting dynamic shared memory implementation ... windows
selecting default "max_connections" ... 100
selecting default "shared_buffers" ... 128MB
selecting default time zone ... Asia/Calcutta
creating configuration files ... ok
running bootstrap script ... ok
performing post-bootstrap initialization ... ok
syncing data to disk ... ok

initdb: warning: enabling "trust" authentication for local connections
initdb: hint: You can change this by editing pg_hba.conf or using the option -A, or --auth-local and --auth-host, the next time you run initdb.

Success. You can now start the database server using:

    ^"C^:^\Program^ Files^\PostgreSQL^\17^\bin^\pg^_ctl^" -D C:/sec -l logfile start


-------------------------------------------------------------------------------------------

command :  pg_ctl -D C:\pri -o "-p 5433" -l logfile start

waiting for server to start.... done
server started

-------------------------------------------------------------------------------------------

command: psql -p 5433 -d postgres -c "create role replicator with replication login password 'repl_pass';"
CREATE ROLE

-------------------------------------------------------------------------------------------
command:  pg_basebackup -D c:\sec -Fp -Xs -P -R -h 127.0.0.1 -U replicator -p 5433
2025-05-14 13:12:51.435 IST [14548] LOG:  checkpoint starting: force wait
2025-05-14 13:12:51.992 IST [14548] LOG:  checkpoint complete: wrote 7 buffers (0.0%); 0 WAL file(s) added, 0 removed, 1 recycled; write=0.444 s, sync=0.008 s, total=0.557 s; sync files=6, longest=0.003 s, average=0.002 s; distance=10557 kB, estimate=10557 kB; lsn=0/2000080, redo lsn=0/2000028
24195/24195 kB (100%), 1/1 tablespace

-------------------------------------------------------------------------------------------

PS C:\Users\gowtham> psql -p 5433 -d postgres


postgres=# create table account (id int , name text);
CREATE TABLE                            ^
postgres=#  insert into account values(1,'gowtham');
INSERT 0 1
postgres=# 2025-05-14 13:17:51.983 IST [14548] LOG:  checkpoint starting: time
2025-05-14 13:17:55.753 IST [14548] LOG:  checkpoint complete: wrote 35 buffers (0.2%); 0 WAL file(s) added, 0 removed, 1 recycled; write=3.726 s, sync=0.024 s, total=3.771 s; sync files=27, longest=0.002 s, average=0.001 s; distance=16507 kB, estimate=16507 kB; lsn=0/301EE50, redo lsn=0/301EDF8
create table rental_log (
postgres(# log_id serial primary key,
postgres(# rental_time timestamp,
postgres(# customer_id int,
postgres(# film_id int,
postgres(# amount Numeric,
postgres(# logged_on timestamp default current_timestamp);
CREATE TABLE
------------------------------------------------------------------------------------------
postgres=# select * from rental_log;
 log_id | rental_time | customer_id | film_id | amount | logged_on
--------+-------------+-------------+---------+--------+-----------
(0 rows)


postgres-# CREATE OR REPLACE PROCEDURE sp_insert_rental_log(
    v_customer_id INT,
    v_film_id INT,
    v_amount INT
)
LANGUAGE plpgsql
AS $$
BEGIN
    INSERT INTO rental_log(rental_time,customer_id,film_id,amount,logged_on)
    VALUES (
        current_timestamp,
        v_customer_id,
        v_film_id,
        v_amount,
        current_timestamp
    );
END;
$$;



CREATE PROCEDURE
-----------------------------------------------------------------------------------------------
postgres=# call sp_insert_rental_log(1,1,1000);
CALL

-----------------------------------------------------------------------------------------------  
postgres=# CREATE OR REPLACE FUNCTION func_log()
RETURNS trigger
LANGUAGE plpgsql
AS $$
BEGIN
    RAISE NOTICE 'A new rental_log with id % is created at %', NEW.log_id, current_timestamp;
    RETURN NEW;
END;
$$;

CREATE FUNCTION
-----------------------------------------------------------------------------------------------

postgres=# 2025-05-14 15:17:52.009 IST [14548] LOG:  checkpoint starting: time
2025-05-14 15:17:55.114 IST [14548] LOG:  checkpoint complete: wrote 29 buffers (0.2%); 
0 WAL file(s) added,
 0 removed, 0 recycled; write=3.057 s, sync=0.025 s, total=3.105 s; sync files=26, 
 longest=0.006 s, average=0.001 s; distance=109 kB, estimate=7924 kB;
  lsn=0/307FD58, redo lsn=0/307FCC8

-----------------------------------------------------------------------------------------------

postgres=# CREATE or replace TRIGGER trig_rental_audit
AFTER Insert ON rental_log
FOR EACH ROW
EXECUTE FUNCTION func_log();

CREATE TRIGGER
-----------------------------------------------------------------------------------------------
postgres=#  call sp_insert_rental_log(1,1,1000);
NOTICE:  A new rental_log with id 4 is created at 2025-05-14 15:20:55.343407+05:30
CALL

-----------------------------------------------------------------------------------------------
postgres=# 2025-05-14 15:22:52.114 IST [14548] LOG:  checkpoint starting: time
2025-05-14 15:22:53.117 IST [14548] LOG:  checkpoint 
complete: wrote 10 buffers (0.1%); 0 WAL file(s) added, 0 removed, 0 recycled; 
write=0.967 s, sync=0.012 s, total=1.003 s; sync files=9, longest=0.006 s, 
average=0.002 s; distance=27 kB, estimate=7134 kB; lsn=0/3086A30, redo lsn=0/30869D8





-------------------------------------------------------------------------------------------
standby server commands:



>> pg_ctl -D c:\sec -o "-p 5435" -l c:\sec\logfile start
waiting for server to start.................................. done
server started

-------------------------------------------------------------------------------------------

>> psql -p 5435 -d postgres


postgres=# select * from account;

 id |  name
----+---------
  1 | gowtham
(1 row)

-------------------------------------------------------------------------------------------


postgres=# select * from rental_log;

 log_id |        rental_time         | customer_id | film_id | amount |         logged_on
--------+----------------------------+-------------+---------+--------+----------------------------
      1 | 2025-05-14 14:54:48.547645 |           1 |       1 |   1000 | 2025-05-14 14:54:48.547645
(1 row)

-------------------------------------------------------------------------------------------


postgres=#  select * from rental_log;

 log_id |        rental_time         | customer_id | film_id | amount |         logged_on
--------+----------------------------+-------------+---------+--------+----------------------------
      1 | 2025-05-14 14:54:48.547645 |           1 |       1 |   1000 | 2025-05-14 14:54:48.547645
      2 | 2025-05-14 15:12:23.428037 |           1 |       2 |   2345 | 2025-05-14 15:12:23.428037
      3 | 2025-05-14 15:13:55.245765 |           1 |       2 |   2345 | 2025-05-14 15:13:55.245765
(3 rows)


------------------------------------------------------------------------------------------------------

All the changes are replicated in secondry server.

