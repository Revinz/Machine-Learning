import numpy as np
import tensorflow as tf
from tensorflow import keras
from keras.models import Sequential
from keras.layers import Dense
from keras.layers.convolutional import Conv2D
from keras.layers.convolutional import MaxPooling2D
from keras.layers.core import Flatten
from numpy import loadtxt
from keras.datasets import fashion_mnist #The MNIST dataset of handwritten digits (0-9) ~60k images
import matplotlib.pyplot as plt

# Method taken from https://www.tensorflow.org/tutorials/keras/classification
# With some small adjustment to fit my needs
def plot_image(i, predictions_array, true_label, img):
  predictions_array, true_label, img = predictions_array, true_label[i], img[i]
  plt.grid(False)
  plt.xticks([])
  plt.yticks([])

  plt.imshow(img, cmap=plt.cm.binary)

  predicted_label = np.max(predictions_array)
  if predicted_label == true_label:
    color = 'blue'
  else:
    color = 'red'

  plt.xlabel("{} ({})".format(np.max(predictions_array),
                                true_label),
                                color=color)

'''
Neural Network properties
'''

activationFnc = "relu" #Works like this: If below 0 => outputs 0. If above 0 => outputs the input
input_shape = (28, 28, 1)
num_hidden_nodes = 128
hidden_layers = 3
epochs = 5 #How many times to run through the data set
batch_size = 10


'''
Setting Up the Training Data
'''

#Load the training data

#images contains the handwriting image data
#labels contains the label - what number is on the image
(images_train, labels_train), (images_test, labels_test) = fashion_mnist.load_data()

images_test_original = images_test

# Normalize the image data
images_train = images_train / 255.0
images_test = images_test / 255.0

print(np.shape(images_test))

#Reshapes from (60k, 28, 28) to (60k, 784) to match the NNs require input shape
images_train = images_train.reshape(images_train.shape[0], 28, 28, 1)
images_test = images_test.reshape(images_test.shape[0], 28, 28, 1)

'''
Building the model
'''

print("Setting up model")
model = Sequential()

#Input layer + the conv layers and pooling
model.add(Conv2D(32, kernel_size=(5,5), input_shape=input_shape, activation=activationFnc, strides=(1,1)))
model.add(MaxPooling2D(pool_size=(2, 2)))
model.add(Conv2D(64, kernel_size=(5,5), activation=activationFnc, strides=(1,1)))
model.add(Flatten());

#Output layer:
#
model.add(Dense(10, activation='softmax'))

#Compile the model:
#   Using Sparse Categorical Cross-Entropy loss because I am not using hot-encoded data e.g the data is not [0, 0, 1]
#   'Adam' is an optimizer that makes it so we don't have to specify the learning-rate, which simplifies the process
#   The metric we are interested for is the accuracy of the classification, therefor we will collect that information
model.compile(loss="sparse_categorical_crossentropy", optimizer='adam', metrics=['accuracy'])

'''
Train the model
'''
print("Training ...")
model.fit(images_train, labels_train, epochs=epochs)

'''
Evaluate
'''

_, accuracy = model.evaluate(images_test, labels_test)
print('Accuracy: %.2f' % (accuracy*100))


'''
Predict
'''

predictions = model.predict_classes(images_test)

# Shows some predictions in a figure
#       Also taken from the source listed at the top
num_rows = 5
num_cols = 6
num_images = num_rows*num_cols
plt.figure(figsize=(2*num_cols, 2*num_rows))
for i in range(num_images):
  plt.subplot(num_rows, num_cols, i+1)
  plot_image(i, predictions[i], labels_test, images_test_original)
plt.tight_layout()
plt.show()


