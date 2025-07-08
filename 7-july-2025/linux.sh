# to change the user to root
sudo -i

#create group
groupadd dev

#add users to gropp
useradd -G dev john
useradd -G dev joe

#create password
passwd john
passwd joe

#create dir
mkdir dev-team

#change ownership
chown :dev /home/dev-team/

#provide write permissions
chmod g+w /home/dev-team/

#restrict others
chmod o-rx dev-team

# exit 
exit
su john

#nav to folder
cd /src/dev

#create file
touch file.txt

#ownership change
chown :dev /src/dev/file.txt

#switch user
exit
su joe

#nav
cd /src/dev

#show permission
ls -l | grep file.txt

#modify file.txt

vim file.txt
-i

~hii 
-wq

#create another group
exit
sudo i
groupadd manager
 useradd -G manager gowtham 



#nav to dev

exit
su gowtham
cd /src/dev

"can't cd to /src/dev"