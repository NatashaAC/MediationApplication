# Meditation Tracker
This application was created for the Passion Project for the class HTTP5204 - Mobile Deveplopment. 
I used Code-First Migrations to create my database, ASP.NET MVC Framework and LINQ to perform 
CRUD operations.

## Description
For my passion project I created an application where users have the ability to track meditation sessions, 
add journal entries for those sessions, and search for mantras to use during their sessions.

## Entity Model Relationships
	*1-1 Between Meditation Sessions and Journal Entries
	*1-M Between Mantras and Meditation Sessions
	*M-M Between Categories and Mantras

## Features
- [X] Searchbar for Mantras 
- [X] Responsive
- [X] Register and Sign Up

## Extra Features/Updates
- [ ] Sort Mantras based on categories
- [ ] Have images related to categories
- [ ] Create timer for Meditation Session
- [ ] Add audio/music when using timer
- [ ] Add validation to forms

### Bugs 
- [ ] Fix issue with wrong start time and end time being logged into database