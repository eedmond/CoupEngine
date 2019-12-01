from enum import Enum

# Using enum class create enumerations
class Role(Enum):
   Ambassador = 0
   Assassin = 1
   Captain = 2
   Contessa = 3
   Duke = 4

isAlive = True

def main():
    global isAlive

    args = input().split()
    assert((args[0].lower() == 'glhf') and (len(args) == 5));

    myId = int(args[1])
    numPlayers = int(args[2])
    activePlayers = range(numPlayers)
    role1 = parse_role(args[3])
    role2 = parse_role(args[4])
    money = 3

    while (isAlive):
        nextInput = input()
        if '?' in nextInput:
            if nextInput[0].lower() == 'r':
                print ('n')

            elif nextInput[0].lower() == 'l':
                print (role_to_string(role2))
                role2 = null

            elif nextInput[0].lower() == 'a':
                if (money < 7):
                    print ("IN")
                    money += 1

                else:
                    playerToCoup = activePlayers[0]
                    if (activePlayers[0] == myId):
                        playerToCoup = activePlayers[1]
                    print ("COUP " + str(playerToCoup))
                    money -= 7

        elif nextInput.lower().startswith("el"):
            isAlive = False
        elif nextInput.lower().startswith("gg"):
            isAlive = False
        elif nextInput.lower().startswith("rip"):
            killedPlayer = int(nextInput[4])
            activePlayers = [player for player in activePlayers if player != killedPlayer]
            

def parse_role(role_str):
    if (role_str.lower() == 'am'):
        return Role.Ambassador
    elif role_str.lower() == 'as':
        return Role.Assassin
    elif role_str.lower() == 'cap':
        return Role.Captain
    elif role_str.lower() == 'con':
        return Role.Contessa
    elif role_str.lower() == 'duke':
        return Role.Duke


if __name__ == "__main__":
    main()