# Diet / food intolorance journal project 
# 1.1.0

# Imports
from datetime import datetime
import time
import foods
import psycopg2
from models import FoodEntry
import heapq
from keyboard import is_pressed
import sys


# When first running, prompt the user for a command in the Terminal. Acts like a homepage 
def userAction():
    print("\nType NEW for a new journal entry\nType REPORT to recieve a food report ")
    action = input("").strip().lower()
    if action == "new":
        # User makes new entry
        createEntry()
    elif action == "report":
        # User requests data:
        getReport()
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
    print(f"Entry created for {entryDate}.")
    print(foods.ingredients["ham"])
    
    navigate()

def userDate():
    while True:
        try:
            dateInput = datetime.strptime(input("Date (mm/dd/yy): ").strip(), "%m/%d/%y")
            break
        except ValueError:
            print("Invalid date. Please try again...")
    
    return dateInput

def userFood():
    newFood = input("Food: ").lower().strip()
    # Add this food and ingredients to system if they are new
    if newFood not in foods.foodMap:
        print("This is a new food. Please enter the ingredients below, separated by commas, to register it in the system.")
        inputIngredients = input().lower()
        newIngredients = [item.strip() for item in inputIngredients.split(',')] # Accounts for whitespace
        for new in newIngredients:
            if new not in foods.ingredients:
                foods.ingredients.update({new : 0})
        
        foods.foodMap[newFood] = newIngredients

    return newFood

def userIngredients(thisFood):
    # Return the ingredients stored in food map item
    print(f"'{thisFood.capitalize()}' contains the following ingredients.")
    print(*foods.foodMap[thisFood], sep=", ")
    # Ask if any ingredients are missing
    inputOtherIngredients = input("Please enter any other ingredients in this food, otherwise hit ENTER: ")
    if inputOtherIngredients:
        # Make a copy to ensure added ingredients get accounted for, even if they are not normally in the food
        allIngredients = foods.foodMap[thisFood]
        otherIngredients = [item.strip() for item in inputOtherIngredients.split(',')] # Accounts for whitespace
        for new in otherIngredients:
            # Add to the output
            allIngredients.append(new)
            # Adds any new ingredients to the set
            if new not in foods.ingredients:
                foods.ingredients.update({new : 0})
        # Ask if the extra ingredients should be saved in this food map item
        toKeep = input("Are these ingredients always in this food?").lower().strip()
        keepBool = True if "yes" in toKeep else False
        if keepBool:
            for i in otherIngredients:
                foods.foodMap[thisFood].append(i)
    # Returns [all normal ingredients + extras]
    return allIngredients

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
    # Use heap queue to get the 5 items that made the user sick the most
    ingredientMap = foods.ingredients
    highest = heapq.nlargest(5, ingredientMap.items(), key=lambda item: item[1])
    for i in highest: print(i)
    # ADD: Some interpretation of data, a way to handle 0 values

    navigate()

# Navigation function for when a user action is finished
def navigate():
    print("Press ESC to exit the program, or press SPACE to return home.")
    while True:
        if is_pressed('esc'):
            clearTerminal()
            time.sleep(0.25)
            sys.exit()
        if is_pressed('space'):
            clearTerminal()
            time.sleep(0.25)
            userAction()
            break

if __name__ == '__main__':
    clearTerminal()
    userAction()