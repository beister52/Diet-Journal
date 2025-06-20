# Diet / food intolorance journal project

# Imports
from datetime import datetime
import foods
import psycopg2
from models import FoodEntry

# Database
conn = psycopg2.connect(
    database="foods",
    host="localhost",
    user="postgres",
    password="spoonful247",
    port="5432"
)

cursor = conn.cursor()


# When first running, prompt the user for a command in the Terminal. Acts like a homepage 
def userAction():
    print("\nType NEW for a new journal entry\nType REPORT to recieve a food report ")
    action = input("")
    if action == "NEW":
        # User makes new entry
        createEntry()
    elif action == "REPORT":
        # User requests data:
        pass
    else:
        print("Command not recognized. Please try again...")
        userAction()
    return

# ANSI escape code for clearing terminal (looks nicer)
def clearTerminal():
    print("\x1b[2J", end="")
    print("\x1b[H", end="")

# For User to create a new food entry
def createEntry():
    clearTerminal()
    print("ENTRY CREATION")
    # Get object info
    entryDate = userDate()
    entryName = userFood()
    entryIngredients = userIngredients(entryName)
    entrySick = userSick(entryIngredients)
    # Create the new food object
    newEntry = FoodEntry(entryDate, entryName, entryIngredients, entrySick)
    print(f"Entry created for {entryDate}. Hit Enter to continue.")
    print(foods.ingredients["ham"])
    input()
    clearTerminal()
    # Return to home
    userAction()

def userDate():
    while True:
        try:
            dateInput = datetime.strptime(input("Date (mm/dd/yy): "), "%m/%d/%y")
            break
        except ValueError:
            print("Invalid date. Please try again...")
    
    return dateInput

def userFood():
    while True:
        try:
            newFood = str(input("Food: ").lower())
            break
        except ValueError:
            print("This is not a valid food. Try again...")

    # Add this food and ingredients to system if they are new
    if newFood not in foods.foodMap:
        print("This is a new food. Please enter the ingredients below, separated by commas, to register it in the system.")
        inputIngredients = input().lower()
        newIngredients = [item.strip() for item in inputIngredients.split(',')] # Accounts for whitespace
        # Update ingredient set and food dictionary
        foods.ingredients.update(newIngredients)
        foods.foodMap[newFood] = newIngredients

    return newFood

def userIngredients(thisFood):
    # Return the ingredients stored in food map item
    print(f"'{thisFood.capitalize()}' contains the following ingredients.")
    print(*foods.foodMap[thisFood], sep=", ")
    # Ask if any ingredients are missing
    inputOtherIngredients = input("Please enter any other ingredients in this food, otherwise hit Enter: ")
    if inputOtherIngredients:
        otherIngredients = [item.strip() for item in inputOtherIngredients.split(',')] # Accounts for whitespace
        foods.ingredients.update(otherIngredients)
        # Ask if these ingredients should be saved in this food map item
        toKeep = input("Are these ingredients always in this food?").lower().strip()
        keepBool = True if "yes" in toKeep else False
        if keepBool:
            for i in otherIngredients:
                foods.foodMap[thisFood].append(i)

    return foods.foodMap[thisFood]

def userSick(ingredients):
    # Ask if the user got sick after eating this food
    inputSick = input("Did this food make you sick? ").lower().strip()
    gotSick = True if "yes" in inputSick else False 
    
    # Increment ingredient map count
    for i in ingredients:
        foods.ingredients[i] += 1

    return gotSick

    
# Return a report of foods/ingredients consumed that made the user feel unwell
def getReport():
    clearTerminal()
    print("YOUR DIET REPORT")

userAction()