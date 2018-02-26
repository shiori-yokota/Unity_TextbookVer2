import sys
sys.path.append(PYTHON_LIB_PATH)
import random
import os.path
if os.path.exists('Python/Sources/qvalues.txt'):
	filename = 'Python/Sources/qvalues.txt'
else:
	filename = ''

SIZE = 5
EPSILON = 0.3
GAMMA = 0.9
BETA = 0.1

# Q値を初期化する
def init_qvalues():
	Q = [[ 0.0 for num_actions in range(4)] for state in range(SIZE * SIZE)]
	return Q

# 行動を選択する
def eGreedy(Q, state):
	rndm_val = random.randint(0, 100)
	best_val = -100000.0
	curr_val = -100000.0
	tmp_act = []
	action = -1
	
	if rndm_val > EPSILON * 100: # (1 - epsilon)の確率 ランダムにactを選択
		action = random.randint(0, 3)
	else: # epsilonの確率 Q値が最大となるようなactionを選択
		for act in range(4):
			curr_val = float(Q[state][act])
			if curr_val > best_val:
				best_val = curr_val
				action = act
				tmp_act.append(action)
			elif curr_val == best_val:
				action = act
				tmp_act.append(action)
				if len(tmp_act) > 0:
					index = len(tmp_act) - 1
					tmpNum = random.randint(0, index)
					action = tmp_act[tmpNum]

	return action

# Q値を更新する
def update_qvalue(Q, old_state, new_state, act, reward):
	best_new_qval = best_qvalue(Q, new_state)
	qval = Q[old_state][act]
	Q[old_state][act] = (1.0 - BETA) * float(qval) + BETA * (reward + GAMMA * float(best_new_qval))
	return Q

def best_qvalue(Q, state):
	best_val = -1000000.0
	for i in range(4):
		if Q[state][i] > best_val:
			best_val = Q[state][i]
	return best_val;

# Q値を記録する
def writeQvalue(Q):
	output = open(filename, 'w')
	for state in range(SIZE * SIZE):
		for action in range(4):
			output.write('qvalue['+str(state)+']['+str(action)+']:'+str(Q[state][action]))
			output.write('\n')
	output.close()

# 記録したQ値を読み込む
def readQvalue(Q):
	input = open(filename, 'r')
	lines = input.readlines()
	value = []
	for line in lines:
		line = line.replace('\n','')
		line = line.replace('\r','')
		itemList = line.split(':')[1]
		value.append(itemList)
	input.close()
	s1 = 0
	s2 = 4
	for i in range(SIZE * SIZE):
		a = value[s1:s2]
		for index in range(len(a)):
			Q[i][index] = a[index]
		s1 += 4
		s2 += 4
	return Q

#### MAIN ####

# Q値を初期化する
if ROBOTSTATE == 'INITIAL':
	qvalue = init_qvalues()
	# Q値の保存
	writeQvalue(qvalue)
	# 行動を選択する -> 移動
	ACTION = eGreedy(qvalue, ROBOTPOSITION)
elif ROBOTSTATE == 'GETREWARD': # 報酬が返ってくる
	qvalue = init_qvalues()
	qvalue = readQvalue(qvalue)
	qvalue = update_qvalue(qvalue, OLDROBOTPOSITION, ROBOTPOSITION, ACTION, REWARD)
	# Q値の保存
	writeQvalue(qvalue)
	# 行動を選択する -> 移動
	ACTION = eGreedy(qvalue, ROBOTPOSITION)
elif ROBOTSTATE == 'ARRIVEDGOAL':
	qvalue = init_qvalues()
	qvalue = readQvalue(qvalue)
	# Q値の保存
	writeQvalue(qvalue)
	# 行動を選択する -> 移動
	ACTION = eGreedy(qvalue, ROBOTPOSITION)
