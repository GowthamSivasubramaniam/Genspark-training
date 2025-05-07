
-- Basic 
select * from cd.facilities;
select name , membercost from cd.facilities;
select * from cd.facilities where membercost>0;

select facid,name,membercost,monthlymaintenance from cd.facilities where
membercost < monthlymaintenance /50 and membercost > 0 ;
select * from cd.facilities where name like '%Tennis%';
select * from cd.facilities where facid in (1,5)

select name , case
 when monthlymaintenance>100 then 'expensive'
 else
 'cheap'
 end as cost
from cd.facilities ;
select memid,surname,firstname,joindate from cd.members where EXTRACT(MONTH FROM joindate) > 8
and EXTRACT(YEAR FROM joindate)>2011
select distinct surname from cd.members order by surname limit 10
select surname from cd.members union select name as surname from cd.facilities;
select max(joindate) as latest from cd.members;

select firstname , surname, joindate from cd.members order by joindate desc limit 1;

-- Joins

select starttime from cd.bookings as b join cd.members as m on m.memid = b.memid
where concat(m.firstname, m.surname) like '%DavidFarrell%';

select starttime start , name from cd.bookings as b 
join cd.facilities as f on b.facid = f.facid 
where Date(starttime) = Date('2012-09-21') and name like '%Tennis Court%' 
order by starttime;

select distinct m1.firstname , m1.surname from cd.members as m1 
join cd.members as m2 on
m1.memid = m2.recommendedby
order by surname , firstname 

select  m1.firstname as memfname, m1.surname as memsname, m2.firstname as recfname, m2.surname as recsname  
from cd.members as m1 
left outer join cd.members as m2 on
m2.memid = m1.recommendedby
order by m1.surname , m1.firstname 

select distinct concat(firstname,' ',surname) member , name facility
  from cd.members m join cd.bookings b on m.memid = b.memid
  join cd. facilities f on b.facid = f.facid where f.name like '%Tennis Court%'
  order by 1 ,2;

select concat(firstname,' ',surname) member , name facility , case
when b.memid = 0 THEN guestcost * slots
else
membercost *slots
end as cost
  from cd.members m join cd.bookings b on m.memid = b.memid
  join cd. facilities f on b.facid = f.facid 
  where date(starttime) >= date('2012-09-14') and date(starttime) < date('2012-09-15')  and  (
			(b.memid = 0 and b.slots*guestcost > 30) or
			(b.memid != 0 and b.slots*membercost > 30)
		) order by 3 desc ;


