f = open("output.txt", "r")

counter = 0

for i in f.readlines():
    pattern = open("meso-pattern.txt", "r")
    for j in pattern.readlines():
        print(j, i)
        if (i == j):
            counter = counter+1

print(counter)