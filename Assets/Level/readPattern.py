pattern_name = "Pattern_level03"

f = open(pattern_name + ".txt", "r")

counter = 0

for i in f.readlines():
    pattern = open("meso-pattern.txt", "r")
    for j in pattern.readlines():
        if (i == j):
            print(i)
            counter = counter+1

print(counter)