import numpy as np
import tensorflow as tf
from tensorflow import keras
from keras.models import Sequential
from keras.layers import Dense
from numpy import loadtxt



'''
Neural Network properties
'''

activationFnc = "relu" #Works like this: If below 0 => outputs 0. If above 0 => outputs the input
num_inputs = 9 #One for each tile
num_hidden_nodes = 32
hidden_layers = 16
epochs = 150
batch_size = 10


'''
Generate Training Data
'''

print("Generating Training Data")
sample_size = 5000 #How many samples to generate
seed = 1 #To make the training data consistent while testing

#Generate the training data
data = loadtxt('ML_Training_data.csv', delimiter=',')

#Split the data into training data and correct output
test_data = data[:, 0:9]
correct_output = data[:, 9]

print(len(data))

'''
Building the model
'''
print("Setting up model")
model = Sequential()

#First hidden layer + input:
model.add(Dense(num_hidden_nodes, input_dim=num_inputs, activation=activationFnc))

#Further hidden layers:
for i in range(hidden_layers - 1):
    model.add(Dense(num_hidden_nodes, activation=activationFnc))

#Output layer:
#   Only 1 (value 1 = has a black line, 0 = does not have a black line)
model.add(Dense(1, activation='sigmoid'))

#Compile the model:
#   Using Binary Cross-Entropy loss because we are only looking for 2 labels ('has a line' and 'does not have a line')
#   'Adam' is an optimizer that makes it so we don't have to specify the learning-rate, which simplifies the process
#   The metric we are interested for is the accuracy of the classification, therefor we will collect that information
model.compile(loss='binary_crossentropy', optimizer='adam', metrics=['accuracy'])

'''
Train the model
'''
print("Training ...")
model.fit(test_data, correct_output, epochs=epochs, batch_size=batch_size)

'''
Evaluate
'''

_, accuracy = model.evaluate(test_data, correct_output)
print('Accuracy: %.2f' % (accuracy*100))


'''
Predict
'''

#Generate the test data
data = loadtxt('ML_Test_data.csv', delimiter=',')

#Split the data into test data and correct output
test_data = data[:, 0:9]
correct_output = data[:, 9]

predictions = model.predict_classes(test_data)

for i in range(len(test_data)):
	print('%s => %d (expected %d)' % (test_data[i].tolist(), predictions[i], correct_output[i]))

