import React, { useState, useRef, useContext } from 'react';
import { ImageBackground, View, Text, TextInput, Image, TouchableOpacity, FlatList, StyleSheet, Dimensions, Alert, ScrollView } from 'react-native';
import { useRoute, useNavigation } from '@react-navigation/native';
import { ThemeContext } from './ThemeContext';

export default CarouselScreen = () => {
  const route = useRoute();
  const user = route.params?.user;
  const navigation = useNavigation();
  const { theme } = useContext(ThemeContext);
  const styles = getStyles(theme);
  const routeTaskValues = route.params?.taskValues || {};
  const data = route.params?.data || [];
  const [taskValues, setTaskValues] = useState(() => {
  const initialValues = {};
    data.forEach(task => {
      initialValues[task.id] = {
        taskId: task.id,
        values: task.type.name === "textbox" 
          ? [""] 
          : Array(task.isCorrect.split(";").length).fill("0")
      };
    });
    return {...routeTaskValues, ...initialValues};
  });
  const flatListRef = useRef(null);
  const [currentIndex, setCurrentIndex] = useState(0);

  const onScroll = (event) => {
    const contentOffset = event.nativeEvent.contentOffset.x;
    const viewSize = event.nativeEvent.layoutMeasurement.width;
    const newIndex = Math.floor(contentOffset / viewSize);
    setCurrentIndex(newIndex);
  };

  const allTasksCompleted = () => {
    return data.every(task => {
      const taskValue = taskValues[task.id];
      if (!taskValue || !taskValue.values) return false;
      
      if (task.type.name === "textbox") {
        return taskValue.values[0]?.trim() !== "";
      } else {
        return taskValue.values.some(val => val === "1");
      }
    });
  };

  const handleFinish = () => {    
    if (!allTasksCompleted()) {
      Alert.alert(
        "Hiányzó válaszok",
        "Kérjük, válaszolj meg minden kérdést a teszt befejezéséhez!",
        [{ text: "OK" }]
      );
      return;
    }
    
    if (!user) {
      Alert.alert("Hiba", "Nincs bejelentkezett felhasználó");
      return;
    }

    navigation.navigate('TestStatistics', {
      taskValues,
      exercises: data,
      user: user
    });
  };
  
  const handleTextboxChange = (id, text, index) => {
    setTaskValues(prev => ({
      ...prev,
      [id]: {
        ...prev[id],
        values: {
          ...(prev[id]?.values || {}),
          [index]: text
        }
      }
    }));
  };

  const handleRadioChange = (taskId, index) => {
    setTaskValues(prev => {
      const newValues = { ...prev };
      if (!newValues[taskId]) {
        newValues[taskId] = {
          values: Array(data.find(t => t.id === taskId).isCorrect.split(";").length).fill("0")
        };
      }
      const newArray = newValues[taskId].values.map((val, i) => 
        i === index ? "1" : "0"
      );
      return {
        ...newValues,
        [taskId]: {
          ...newValues[taskId],
          values: newArray
        }
      };
    });
  };

  const handleCheckboxChange = (taskId, index) => {
    setTaskValues(prev => ({
      ...prev,
      [taskId]: {
        ...prev[taskId],
        values: prev[taskId].values.map((val, i) => 
          i === index ? (val === "1" ? "0" : "1") : val
        )
      }
    }));
  };

  const renderFinishButton = () => {
    if (currentIndex === data.length - 1) {
      return (
        <TouchableOpacity style={styles.finishButton} onPress={handleFinish}>
          <Text style={styles.finishButtonText}>Teszt befejezése</Text>
        </TouchableOpacity>
      );
    }
    return null;
  };

  return (
    <View style={styles.container}>
      <ImageBackground source={require('../background.jpg')} style={styles.background} resizeMode="cover">
        <FlatList
          ref={flatListRef}
          data={data}
          horizontal
          pagingEnabled
          showsHorizontalScrollIndicator={false}
          onScroll={onScroll}
          scrollEventThrottle={16}
          keyExtractor={(item, index) => index.toString()}
          scrollEnabled={true}
          renderItem={({ item: elem, index }) => (
            <ScrollView 
              contentContainerStyle={styles.scrollContainer}
              nestedScrollEnabled={true}
            >
              <View style={styles.card}>
                {/* Card Header */}
                <View style={styles.cardHeader}>
                  <Text style={styles.cardTitle}>
                    {index + 1}. feladat{"\n"}{elem.description}
                  </Text>
                  <Text style={styles.cardText}>{elem.text}</Text>
                </View>

                {/* Image if exists */}
                {elem.picName && (
                  <TouchableOpacity
                    style={styles.imageContainer}
                  >
                    <Image
                      style={styles.taskImage}
                      source={{ uri: `https://res.cloudinary.com/drpkpopsq/image/upload/v1741078235/${elem.picName}` }}
                      resizeMode="contain"
                    />
                  </TouchableOpacity>
                )}

                {/* Inputs Section */}
                <View style={styles.taskInputs}>
                  {/* Textbox Input */}
                  { elem.type.name === "textbox" && (
                    elem.isCorrect.split("|").map((_, index) => (
                      <View key={index}>
                        { _.split('_').length > 1 ? (
                          <>
                            <Text>{_.split('_')[0]}</Text>
                            <TextInput
                              style={styles.textbox}
                              value={taskValues[elem.id]?.values?.[index] || ""}
                              onChangeText={(text) => handleTextboxChange(elem.id, text, index)}
                              placeholder="Írd ide a válaszod..."
                              placeholderTextColor="#666"
                              multiline={false}
                            />
                          </>
                        ) : (
                          <TextInput
                            style={styles.textbox}
                            value={taskValues[elem.id]?.values?.[index] || ""}
                            onChangeText={(text) => handleTextboxChange(elem.id, text, index)}
                            placeholder="Írd ide a válaszod..."
                            placeholderTextColor="#666"
                            multiline={false}
                          />
                        )}
                      </View>
                    ))
                  )}

                  {/* Radio Buttons */}
                  {elem.type.name === "radio" && (
                    <View style={styles.radioContainer}>
                      {elem.answers.split(";").map((answer, index) => (
                        <TouchableOpacity
                          key={index}
                          style={styles.radioOption}
                          onPress={() => handleRadioChange(elem.id, index)}
                        >
                          <View style={styles.radioCircle}>
                            {taskValues[elem.id]?.values?.[index] === "1" && <View style={styles.radioDot} />}
                          </View>
                          <Text style={styles.radioLabel}>{answer}</Text>
                        </TouchableOpacity>
                      ))}
                    </View>
                  )}

                  {/* Checkboxes */}
                  {elem.type.name === "checkbox" && (
                    <View style={styles.checkboxContainer}>
                      {elem.answers.split(";").map((answer, index) => (
                        <TouchableOpacity
                          key={index}
                          style={styles.checkboxOption}
                          onPress={() => handleCheckboxChange(elem.id, index)}
                        >
                          <View style={styles.checkboxBox}>
                            {taskValues[elem.id]?.values?.[index] === "1" && (
                              <Text style={styles.checkboxTick}>✓</Text>
                            )}
                          </View>
                          <Text style={styles.checkboxLabel}>{answer}</Text>
                        </TouchableOpacity>
                      ))}
                    </View>
                  )}
                </View>


                {/* Progress indicator and finish button */}
                {renderFinishButton()}
              </View>
            </ScrollView>
          )}
        />
        
      </ImageBackground>
    </View>
  );
};

const getStyles = (theme) => StyleSheet.create({
  container: {
    flex: 1,
    justifyContent: 'center',
    alignItems: 'center',
  },
  card: {
    width: Dimensions.get('window').width - 10,
    minHeight: Dimensions.get('window').height * 0.75,
    backgroundColor: theme,
    borderRadius: 10,
    padding: 15,
    marginHorizontal: 5,
    shadowColor: "#000",
    shadowOffset: {
      width: 0,
      height: 2,
    },
    shadowOpacity: 0.25,
    shadowRadius: 3.84,
    elevation: 5,
    top: 45,
    marginBottom: 65,
  },
  cardHeader: {
    marginBottom: 15,
  },
  cardTitle: {
    color: 'white',
    fontSize: 18,
    fontWeight: 'bold',
    textAlign: 'center',
    marginBottom: 10,
  },
  cardText: {
    color: 'white',
    textAlign: 'center',
    marginBottom: 15,
  },
  imageContainer: {
    justifyContent: 'center',
    alignItems: 'center',
    marginBottom: 15,
  },
  taskImage: {
    width: '100%',
    height: 200,
    borderRadius: 5,
  },
  taskInputs: {
    marginTop: 10,
  },
  textbox: {
    width: '100%',
    height: 40,
    backgroundColor: 'white',
    color: 'black',
    borderWidth: 1,
    borderColor: '#ccc',
    borderRadius: 5,
    paddingHorizontal: 10,
    marginBottom: 10,
  },
  radioContainer: {
    flexDirection: 'row',
    flexWrap: 'wrap',
    justifyContent: 'space-between',
    width: '96%',
  },
  radioOption: {
    flexDirection: 'row',
    alignItems: 'center',
    width: '45%',
    marginBottom: 10,
  },
  radioCircle: {
    height: 20,
    width: 20,
    borderRadius: 10,
    borderWidth: 2,
    borderColor: '#fff',
    alignItems: 'center',
    justifyContent: 'center',
    marginRight: 10,
  },
  radioDot: {
    height: 10,
    width: 10,
    borderRadius: 5,
    backgroundColor: '#fff',
  },
  radioLabel: {
    color: 'white',
  },
  checkboxContainer: {
    flexDirection: 'row',
    flexWrap: 'wrap',
    justifyContent: 'space-between',
    width: '96%',
  },
  checkboxOption: {
    flexDirection: 'row',
    alignItems: 'center',
    width: '45%',
    marginBottom: 10,
  },
  checkboxBox: {
    height: 20,
    width: 20,
    borderWidth: 2,
    borderColor: '#fff',
    alignItems: 'center',
    justifyContent: 'center',
    marginRight: 10,
  },
  checkboxTick: {
    color: '#fff',
    fontSize: 14,
  },
  checkboxLabel: {
    color: 'white',
  },
  progressContainer: {
    position: 'absolute',
    top: 10,
    right: 20,
    backgroundColor: 'rgba(0,0,0,0.5)',
    padding: 8,
    borderRadius: 20,
  },
  progressText: {
    color: 'white',
    fontWeight: 'bold',
  },
  finishButton: {
    alignSelf: 'center',
    backgroundColor: '#27ae60',
    paddingVertical: 12,
    paddingHorizontal: 30,
    borderRadius: 25,
    elevation: 5,
    marginTop: 15,
  },
  finishButtonText: {
    color: 'white',
    fontWeight: 'bold',
    fontSize: 16,
  },
});
