f = open("output.txt", "r")


# for i in f.readlines():
#     pattern = open("meso-pattern.txt", "r")
#     for j in pattern.readlines():
#         for i

for i in f.readlines():
    for j in i:
        print(j)