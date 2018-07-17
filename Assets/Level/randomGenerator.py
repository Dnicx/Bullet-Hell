import random

groups = ["A", "B", "C", "D", "E", "F", "G", "H", "I"]

each_prop = [1, 1, 1, 1, 1, 1, 1, 1, 1]

#length in minute
level_length = 1
detail = 4
density = 7


for lv in range(1, 21):
    time = 1.0
    level_name = "level"
    if (lv < 10):
         level_name = level_name+"0"
    level_name = level_name+str(lv)

    level = open(level_name + ".txt", "a")

    pool = ""
    for j in range(0, len(groups)):
        pool += groups[j]*each_prop[j]
    while (len(pool) < 400):
        pool += "-"

    game_time = level_length * 60 * detail
    for i in range(0, game_time):
        line = ""
        line += "%.1f," % time
        time += 1/detail
        for j in range(0, density):
            # select = random.randrange(0, len(groups), 1)
            # if (random.randrange(1, 100, 1) <= each_prop[select]):
            #     line += groups[select]
            # else:
            #     line += "-"
            line += random.sample(pool, 400)[0]
        line += "\n"
        level.write(line)

