--Phase 2: DDL & DML

-- Create all tables with appropriate constraints (PK, FK, UNIQUE, NOT NULL)
-- Insert sample data using `INSERT` statements
-- Create indexes on `student_id`, `email`, and `course_id`

create table students(
student_id serial constraint pk_student_id primary key,
student_name varchar(40) not null ,
email varchar(100) unique not null,
phone varchar(10) unique not null
);

create table courses
(
course_id serial constraint pk_course_id primary key,
course_name varchar(100) not null,
category varchar(100) not null,
duration_days int not null constraint days_check check(duration_days > 0)
);

create table trainers
(
trainer_id serial constraint pk_trainer_id primary key,
trainer_name varchar(100) not null,
expertise varchar(100) not null
);

create table enrollments
(
enrollment_id serial constraint pk_enrollment_id primary key,
student_id int not null,
course_id int not null,
enroll_date date not null,
constraint fk_student_id_enrollments foreign key(student_id)  references students(student_id),
constraint fk_course_id_enrollments foreign key(course_id) references courses(course_id)
);

create table certificates
(
certificate_id serial constraint pk_certificate_id primary key,
enrollment_id int not null,
issue_date date not null,
serial_no varchar(10) unique not null ,
constraint fk_enrollment_id_certificates  foreign key(enrollment_id) 
references enrollments(enrollment_id)
);

create table courses_trainers
(
trainer_id int not null,
course_id int not null,
enroll_date date not null,
constraint fk_trainer_id_courses_trainers foreign key(trainer_id)  references trainers(trainer_id),
constraint fk_course_id_courses_trainers foreign key(course_id)  references courses(course_id)
);

create index students_index on enrollments(student_id);
create index courses_index  on enrollments(course_id);
create index courses_trainers_trainer_index on courses_trainers(trainer_id);
create index courses_trainers_courses_index  on courses_trainers(course_id);
create index enrollment_index  on certificates(enrollment_id);
create index serial_no_index  on certificates(serial_no);
create index email_index  on students(email);

INSERT INTO students (student_name, email, phone) VALUES
('Arun Kumar', 'arun.kumar@gmail.com', '9876543210'),
('Divya Lakshmi', 'divya.lakshmi@gmail.com', '9876543211'),
('Sathish R', 'sathish.r@gmail.com', '9876543212'),
('Kavya S', 'kavya.s@gmail.com', '9876543213'),
('Vignesh M', 'vignesh.m@gmail.com', '9876543214'),
('Preethi V', 'preethi.v@gmail.com', '9876543215'),
('Murugan K', 'murugan.k@gmail.com', '9876543216'),
('Revathi R', 'revathi.r@gmail.com', '9876543217'),
('Prakash Babu', 'prakash.babu@gmail.com', '9876543218'),
('Meena Kumari', 'meena.kumari@gmail.com', '9876543219');


INSERT INTO courses (course_name, category, duration_days) VALUES
('Python Basics', 'Programming', 30),
('Advanced Java', 'Programming', 45),
('Web Development', 'IT', 60),
('Database Design', 'Database', 40),
('Cloud Fundamentals', 'Cloud', 50),
('Data Science', 'AI', 70),
('Networking', 'IT', 35),
('Cybersecurity', 'Security', 55),
('AI with Python', 'AI', 65),
('DevOps Essentials', 'DevOps', 45);


INSERT INTO trainers (trainer_name, expertise) VALUES
('Karthik Raj', 'Python'),
('Sowmya Iyer', 'Java'),
('Ramesh S', 'Web Development'),
('Janani V', 'Database'),
('Saravanan P', 'Cloud'),
('Deepika M', 'AI'),
('Balaji K', 'Networking'),
('Sangeetha R', 'Security'),
('Natarajan L', 'AI'),
('Uma Maheswari', 'DevOps');


INSERT INTO enrollments (student_id, course_id, enroll_date) VALUES
(5, 3, '2024-01-12'),
(2, 7, '2024-02-25'),
(9, 1, '2023-12-18'),
(1, 10, '2024-01-30'),
(3, 4, '2024-02-15'),
(6, 9, '2024-03-11'),
(7, 6, '2024-04-02'),
(8, 2, '2024-01-09'),
(4, 8, '2024-02-02'),
(10, 5, '2023-12-28'),
(2, 1, '2024-03-20'),
(1, 3, '2024-04-04'),
(6, 6, '2024-02-19'),
(9, 4, '2024-01-25'),
(3, 2, '2024-03-14'),
(8, 7, '2024-01-21'),
(7, 10, '2023-12-10'),
(4, 5, '2024-03-01'),
(10, 9, '2024-02-27'),
(5, 8, '2024-01-15'),
(1, 1, '2023-12-14'),
(9, 6, '2024-03-25'),
(6, 2, '2024-02-08'),
(2, 4, '2024-01-18'),
(3, 9, '2024-04-11'),
(8, 5, '2024-01-06'),
(7, 1, '2024-03-05'),
(4, 3, '2024-02-04'),
(10, 7, '2024-03-10'),
(5, 10, '2024-04-07'),
(1, 6, '2023-12-22'),
(6, 8, '2024-01-27'),
(2, 10, '2024-03-02'),
(9, 5, '2024-02-22'),
(7, 3, '2024-01-03'),
(3, 7, '2024-03-28'),
(4, 1, '2024-04-14'),
(10, 2, '2024-01-19'),
(8, 4, '2023-12-16'),
(5, 9, '2024-02-12'),
(6, 5, '2024-03-16'),
(1, 7, '2024-01-01'),
(9, 10, '2024-02-09'),
(2, 3, '2024-04-01'),
(7, 9, '2023-12-05'),
(4, 6, '2024-01-23'),
(10, 8, '2024-03-07'),
(8, 1, '2024-02-17'),
(3, 10, '2024-01-11'),
(5, 2, '2023-12-26');

INSERT INTO certificates (enrollment_id, issue_date, serial_no) VALUES
(3, '2024-01-05', 'CERT00001'),
(7, '2024-04-08', 'CERT00002'),
(15, '2024-01-28', 'CERT00003'),
(21, '2023-12-20', 'CERT00004'),
(9, '2024-02-08', 'CERT00005'),
(12, '2024-04-10', 'CERT00006'),
(18, '2024-03-10', 'CERT00007'),
(25, '2024-01-20', 'CERT00008'),
(30, '2024-04-10', 'CERT00009'),
(35, '2024-04-16', 'CERT00010'),
(2, '2024-03-01', 'CERT00011'),
(6, '2024-03-15', 'CERT00012'),
(14, '2024-01-30', 'CERT00013'),
(20, '2024-01-17', 'CERT00014'),
(29, '2024-04-08', 'CERT00015'),
(32, '2024-03-06', 'CERT00016'),
(36, '2024-04-20', 'CERT00017'),
(41, '2024-01-03', 'CERT00018'),
(45, '2024-01-25', 'CERT00019'),
(49, '2024-02-20', 'CERT00020');

INSERT INTO courses_trainers (trainer_id, course_id, enroll_date) VALUES
(1, 3, '2024-01-05'),
(2, 5, '2024-03-10'),
(3, 2, '2024-02-14'),
(4, 7, '2024-04-01'),
(5, 1, '2024-01-20'),
(6, 6, '2024-03-25'),
(7, 4, '2023-12-15'),
(8, 8, '2024-02-28'),
(9, 9, '2024-04-05'),
(10, 10, '2024-01-30'),
(1, 2, '2024-03-15'),
(2, 4, '2023-11-22'),
(3, 6, '2024-02-10'),
(4, 1, '2024-04-17'),
(5, 7, '2024-01-12'),
(6, 3, '2024-03-18'),
(7, 9, '2024-04-25'),
(8, 5, '2024-02-02'),
(9, 10, '2023-12-10'),
(10, 8, '2024-03-27');


--Phase 3: SQL Joins Practice


--1. List students and the courses they enrolled in


select student_name ,course_name from enrollments e 
join students s on e.student_id = s.student_id 
join courses c on c.course_id = e.course_id 
order by student_name ; 



-- 2. Show students who received certificates with trainer names


select student_name , course_name , serial_no , trainer_name from
enrollments e join certificates c on e.enrollment_id = c.enrollment_id
join students s on s.student_id = e. student_id
join courses co on co.course_id = e.course_id
join courses_trainers ct on co.course_id = ct.course_id
join trainers t on t.trainer_id = ct.trainer_id order by serial_no;



--3. Count number of students per course

select course_name , count(*) as enrollment_count 
from courses e 
left join enrollments co on co.course_id = e.course_id
group by e.course_id,course_name order by 2


--phase 4 

--Create `get_certified_students(course_id INT)
-- → Returns a list of students who completed the given course and received certificates.

create or replace function get_certified_students(v_course_id INT)
returns table(
    student_name varchar,
    certificate varchar(10)
)
language plpgsql
as $$
begin
return query
select s.student_name , serial_no as certificate from
enrollments e join certificates c on e.enrollment_id = c.enrollment_id
join students s on s.student_id = e. student_id where e.course_id=v_course_id;
end;
$$;

select * from get_certified_students(10);

-- Create `sp_enroll_student(p_student_id, p_course_id)`
-- Inserts into `enrollments` and conditionally adds a certificate if completed (simulate with status flag).


create or replace procedure sp_enroll_student(p_student_id Int , p_course_id int ,flag boolean)
language plpgsql
as $$
declare
v_enroll_id int;
max_v_certno varchar;
begin
	begin
 		insert into enrollments(student_id,course_id,enroll_date) 
 		values (p_student_id,p_course_id,current_timestamp::date) returning enrollment_id into v_enroll_id;
		 
        raise notice 'Inserted enrollment and the enrollment id is %',v_enroll_id;

		
		if flag then
  			select 'CERT' || LPAD((CAST(substring(serial_no FROM 5) AS INT) + 1)::TEXT, 5, '0') 
    		into max_v_certno from certificates order by serial_no desc limit 1;
			
			if max_v_certno is null then
                max_v_certno := 'CERT00001';
            end if;
			
  			insert into certificates (enrollment_id, issue_date, serial_no) values
  			(v_enroll_id,current_timestamp::date,max_v_certno);
			raise notice 'Inserted certificate and certificate_no is %',max_v_certno;
		end if;
	exception when others then
 		raise notice 'error %',sqlerrm;
		raise notice 'insert failed';
	end;
end;
$$;



call sp_enroll_student(1,10,false);


/* Phase 5: Cursor
Use a cursor to:

* Loop through all students in a course
* Print name and email of those who do not yet have certificates

*/
create or replace procedure sp_fetch_student_without_certificates(v_course_id int)
language plpgsql as
$$
declare 
rec record;
cert_cur cursor for 
select student_name , email  from students s
left join enrollments e on s.student_id = e.student_id 
left join certificates c on c.enrollment_id = e.enrollment_id 
where course_id =2 and serial_no is null;

begin 
 open cert_cur;
 loop
  fetch from cert_cur into rec;
  exit when not found;

   raise notice 'Student: %, Email: %', rec.student_name, rec.email;

 end loop;
 close cert_cur;
end;
$$;

call sp_fetch_student_without_certificates(2);

/*Phase 6: Security & Roles

1. Create a `readonly_user` role:

   * Can run `SELECT` on `students`, `courses`, and `certificates`
   * Cannot `INSERT`, `UPDATE`, or `DELETE`
   

2. Create a `data_entry_user` role:

   * Can `INSERT` into `students`, `enrollments`
   * Cannot modify certificates directly

*/

create role read_only login password '1234';
revoke all ON students, courses, certificates from public;
grant select on students, courses, certificates to read_only;
create role data_entry_user login password '1234';
grant insert on students, enrollments to  data_entry_user
grant usage on sequence enrollments_enrollment_id_seq to data_entry_user;
grant usage on sequence students_student_id_seq to data_entry_user


--Phase 7: Transactions & Atomicity

--Write a transaction block that:

-- Enrolls a student
-- Issues a certificate
-- Fails if certificate generation fails (rollback)



create or replace procedure sp_enroll(p_student_id Int , p_course_id int )
language plpgsql
as $$
declare
v_enroll_id int;
max_v_certno varchar;
begin
	begin
 		insert into enrollments(student_id,course_id,enroll_date) 
 		values (p_student_id,p_course_id,current_timestamp::date) returning enrollment_id into v_enroll_id;
		 
        raise notice 'Inserted enrollment and the enrollment id is %',v_enroll_id;

		
		
  		select 'CERT' || LPAD((CAST(substring(serial_no FROM 5) AS INT) + 1)::TEXT, 5, '0') 
    	into max_v_certno from certificates order by serial_no desc limit 1;
			
		if max_v_certno is null then
              max_v_certno := 'CERT00001';
        end if;
			
  		insert into certificates (enrollment_id, issue_date, serial_no) values
  		(v_enroll_id,current_timestamp::date,max_v_certno);
		raise notice 'Inserted certificate and certificate_no is %',max_v_certno;
		
	exception when others then
 		raise notice 'error %',sqlerrm;
		raise notice 'insert failed';
		rollback;
	end;
end;
$$;
call sp_enroll(12,10);