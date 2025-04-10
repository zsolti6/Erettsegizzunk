import { useContext } from 'react';
import { ImageBackground, StyleSheet, View, Text, TouchableOpacity, Image } from 'react-native';
import { useNavigation } from '@react-navigation/native';
import Icon from 'react-native-vector-icons/FontAwesome';
import { ThemeContext } from './ThemeContext';

export default MainMenu = () => {
  const navigation = useNavigation();
  const { theme } = useContext(ThemeContext);
  const styles = getStyles(theme);

  const handleSelector = () => {
    navigation.navigate('Selector');
  };

  const handleStatistics = () => {
    navigation.navigate('Statistics');
  };

  const handleSettingsPress = () => {
    navigation.navigate('Settings');
  };

  const handleInformation = () => {
    navigation.navigate('Information');
  };

  return (
    <View style={styles.container}>
      <ImageBackground source={require('../background.jpg')} style={styles.background} resizeMode="cover" >
        <TouchableOpacity style={styles.circleButton} onPress={handleInformation}>
          <Text style={styles.circleButtonText}>i</Text>
        </TouchableOpacity>
        <TouchableOpacity onPress={handleSettingsPress} style={styles.settings}>
          <Icon name="gear" style={styles.settingsIcon} />
        </TouchableOpacity>
        <Image source={require('../erettlogo.png')} style={styles.image} />
        <TouchableOpacity style={styles.button} onPress={handleSelector}>
          <Text style={styles.buttonText}>Új feladatlap</Text>
        </TouchableOpacity>
        <TouchableOpacity style={styles.button} onPress={handleStatistics}>
          <Text style={styles.buttonText}>Statisztika</Text>
        </TouchableOpacity>
      </ImageBackground>
    </View>
  );
};

const getStyles = (theme) =>
  StyleSheet.create({
  container: {
    flex: 1,
    justifyContent: 'center',
    alignItems: 'center',
  },
  settings: {
    position: 'absolute',
    top: 45,
    right: 20,
    width: 50,
    height: 50,
    justifyContent: 'center',
    alignItems: 'center',
  },
  settingsIcon: {
      fontSize: 50,
      color: theme,
    },
  button: {
    backgroundColor: theme,
    padding: 15,
    borderRadius: 5,
    marginVertical: 10,
    width: '80%',
    alignItems: 'center',
  },
  buttonText: {
     color: '#fff',
    fontSize: 18,
  },
  circleButton: {
    position: 'absolute',
    top: 45,
    left: 20,
    width: 50,
    height: 50,
    borderRadius: 25,
    backgroundColor: theme,
    justifyContent: 'center',
    alignItems: 'center',
  },
  circleButtonText: {
    color: '#ffffff',
    fontSize: 20,
    fontWeight: 'bold',
  },
  background: {
    flex: 1,
    width: '100%',
    height: '100%',
    justifyContent: 'center',
    alignItems: 'center',
  },
  image:{
    height: 100,
    width: '80%',
    resizeMode: 'contain',
    position: 'absolute',
    top: 150,
  }
});