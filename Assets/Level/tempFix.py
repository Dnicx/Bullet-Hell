import time
pattern_name = "evolve.txt"


counter = 0
gen = 0
getGen = 0
counter = 0

while (True):
    writeback = ""
    f = open(pattern_name, "r")
    fix = ""
    for i in f.readlines():
        if (len(i) < 10) :
            getGen = int(i[0:len(i)-1])
            writeback = writeback + i
        else :
            if (i[0] == '1'):
                writeback = writeback + "0" + i[1:len(i)]
            else:
                writeback = writeback + i
    f.close() 
    if (getGen == gen):
        counter = counter+1
        if (counter>3):
            fix = " fixed"
            w = open(pattern_name, "w") 
            w.write(writeback)
            w.close()
            counter = 0
    else:
        counter = 0
    print("" + str(getGen) + "-" + str(counter) + fix)
    gen = getGen 
    time.sleep(30)