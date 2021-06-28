# Meditation Tracker
This application was created for the Passion Project for the class HTTP5204 - Mobile Development. 
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
- [X] Add, Edit and Delete Sessions, Entries, Mantras and Categories as a Registered User
- [X] View a List of Sessions and Journal Entries as a Registered User
- [X] View a List of Mantras and Categories as non-registered User
- [X] Searchbar for Mantras
- [X] Assign and UnAssign Mantras to Categories
- [X] Register and Sign Up

## Images
![Home Page](/MeditationApplication/Content/image/home.jpg)
![Session Selector](/MeditationApplication/Content/image/session_selector.jpg)
![Time Selector](/MeditationApplication/Content/image/time_selector.jpg)
![Colour Selector](/MeditationApplication/Content/image/colour_selector.jpg)
![Date Selector](/MeditationApplication/Content/image/date_selector.jpg)

## Extra Features/Updates
- [ ] Add filter that sorts Mantras based on categories
- [ ] Arrange Meditation Sessions and Journal Entries by Date (Descending Order)
- [ ] Have images related to categories
- [ ] Create timer for Meditation Session
- [ ] Add audio/music when using timer
- [ ] Add validation to forms

### Bugs 
- [ ] Fix issue with wrong start time and end time being logged into database
