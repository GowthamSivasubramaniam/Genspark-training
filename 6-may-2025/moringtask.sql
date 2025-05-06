select title as Title_name from titles;
select title as Title_name from titles where pub_id = 1389; 
select * from titles where price between 10 and 15;
select * from titles where price is null;
select * from titles where title like 'the%';
select title from titles where title not like '%v%';
select title from titles order by royalty desc;
select title from titles order by pub_id desc , type asc ,price desc;
select avg(price) as Average , type from titles group by type;
select distinct type from titles;
select top 2 title from titles order by price desc
select title from titles where price <20 and advance > 7000;
SELECT t.title, p.num, t.pub_id
FROM titles t
JOIN (
    SELECT pub_id, COUNT(title) AS num
    FROM titles
    GROUP BY pub_id
    HAVING COUNT(title) > 2
) p ON t.pub_id = p.pub_id
WHERE t.title LIKE '%it%' and price between 15 and 25
ORDER BY p.num;
select * from authors where state = 'CA';
select state,count(state) as count_in_state from authors group by state;


-----------------------------------------------------------------------------------------------------------------------------

create table categories ( id int primary key
, name varchar(100) not null);
create table country ( id int primary key
, name varchar(100)  not null);
create table state ( id int primary key
, name varchar(100)  not null, country_id int, foreign key(country_id) references country(id));
create table city ( id int primary key
, name varchar(100)  not null, state_id int, foreign key(state_id) references state(id));
create table area ( zipcode varchar(10) primary key
, name varchar(100)  not null, city_id int, foreign key(city_id) references city(id));
create table address ( id int primary key
, door_no varchar(10) not null, zipcode varchar(10), foreign key(zipcode) references area(zipcode));
create table supplier ( id int primary key
, name varchar(100) not null,contact_person varchar(100) not null, phone varchar(10),email varchar(100) not null, address_id int ,  foreign key(address_id) references address(id));

CREATE TABLE product (
    id INT PRIMARY KEY,                      -- Unique ID for each product
    name VARCHAR(100) NOT NULL,               -- Name of the product
    unit_price DECIMAL(10, 2) NOT NULL,       -- Price per unit of the product
    quantity INT NOT NULL,                    -- Available quantity of the product
    description TEXT,                         -- Description of the product
    image VARCHAR(255)                        -- Image URL or file path for the product
);


CREATE TABLE product_supplier (
    transaction_id INT PRIMARY KEY,           -- Unique transaction ID
    product_id INT,                           -- Foreign key to product table
    supplier_id INT,                          -- Foreign key to supplier table
    date_of_supply DATE NOT NULL,             -- Date of supply of the product
    quantity INT NOT NULL,                    -- Quantity supplied in the transaction
    FOREIGN KEY (product_id) REFERENCES product(id),     -- Foreign key reference to the product table
    FOREIGN KEY (supplier_id) REFERENCES supplier(id)    -- Foreign key reference to the supplier table
);


CREATE TABLE customer (
    id INT PRIMARY KEY,                      -- Unique ID for each customer
    name VARCHAR(100) NOT NULL,               -- Name of the customer
    phone VARCHAR(15),                        -- Phone number of the customer
    age INT,                                  -- Age of the customer
    address_id INT,                           -- Foreign key to address table
    FOREIGN KEY (address_id) REFERENCES address(id)  -- Foreign key reference to the address table
);

CREATE TABLE orders (
    order_number INT PRIMARY KEY,             -- Unique order number
    customer_id INT,                          -- Foreign key to customer table
    date_of_order DATE NOT NULL,              -- Date when the order was placed
    amount DECIMAL(10, 2) NOT NULL,           -- Total order amount
    order_status VARCHAR(50),                 -- Status of the order (e.g., pending, completed, canceled)
    FOREIGN KEY (customer_id) REFERENCES customer(id)  -- Foreign key reference to the customer table
);

CREATE TABLE order_details (
    id INT PRIMARY KEY,                       -- Unique ID for the order detail record
    order_number INT,                         -- Foreign key to the order table
    product_id INT,                           -- Foreign key to product table
    quantity INT NOT NULL,                    -- Quantity of the product in the order
    unit_price DECIMAL(10, 2) NOT NULL,       -- Price per unit of the product at the time of the order
    FOREIGN KEY (order_number) REFERENCES orders(order_number), -- Foreign key reference to the order table
    FOREIGN KEY (product_id) REFERENCES product(id)           -- Foreign key reference to the product table
);


ALTER TABLE product
ADD category_id INT;


ALTER TABLE product
ADD CONSTRAINT fk_category
FOREIGN KEY (category_id) REFERENCES categories(id);
