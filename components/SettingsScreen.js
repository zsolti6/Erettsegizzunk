import { useContext } from 'react';
import { ImageBackground, View, Text, TouchableOpacity, StyleSheet, Alert } from 'react-native';
import { ThemeContext } from './ThemeContext';
import { useNavigation } from '@react-navigation/native';
import Icon from 'react-native-vector-icons/FontAwesome';

export default SettingsScreen = ({ user, handleLogout, setUser }) => {
  const { theme, updateTheme } = useContext(ThemeContext);
  const styles = getStyles(theme);
  const navigation = useNavigation();

  const themes = [
    { value: '#303D5C', name: 'manatee' },
    { value: '#00668A', name: 'darkblue' },
    { value: '#0091A2', name: 'cyan' },
    { value: '#00BA9C', name: 'lightblue' },
    { value: '#89DE84', name: 'lightgreen' },
    { value: '#006400', name: 'darkgreen' },
    { value: '#D6A419', name: 'gold' },
    { value: '#F9f871', name: 'yellow' },
    { value: '#FFEECA', name: 'sand' },
    { value: '#EB9FC9', name: 'pink' },
    { value: '#C28BB9', name: 'lightpurple' },
    { value: '#9A77A5', name: 'darkpurple' },
  ];

  const handleLogoutPress = async () => {
    Alert.alert(
      'Kijelentkezés',
      'Biztosan ki szeretnél jelentkezni?',
      [
        {
          text: 'Mégse',
          style: 'cancel',
        },
        {
          text: 'Kijelentkezés',
          onPress: async () => {
            await handleLogout();
          },
        },
      ],
      { cancelable: false }
    );
  };

  return (
    <View style={styles.container}>
      <ImageBackground source={require('../background.jpg')} style={styles.background} resizeMode="cover" >
        <TouchableOpacity style={styles.backButton} onPress={() => navigation.goBack()}>
          <Icon name="arrow-left" style={styles.settingsIcon} />
        </TouchableOpacity>

      <View style={styles.authButtonsContainer}>
        {user && user.isGuest ? (
          <>
            <TouchableOpacity 
              style={styles.authButton} 
              onPress={() => navigation.navigate('Login')}
            >
              <Text style={styles.authButtonText}>Bejelentkezés</Text>
            </TouchableOpacity>
            <TouchableOpacity 
              style={styles.authButton} 
              onPress={() => navigation.navigate('Registry')}
            >
              <Text style={styles.authButtonText}>Regisztráció</Text>
            </TouchableOpacity>
          </>
        ) : user ? (
          <>
            <TouchableOpacity 
                style={styles.authButton} 
                onPress={() => navigation.navigate('Profile', {user, setUser})}
              >
                <Text style={styles.authButtonText}>Profil</Text>
            </TouchableOpacity>
            <TouchableOpacity 
              style={styles.authButton} 
              onPress={handleLogoutPress}
            >
              <Text style={styles.authButtonText}>Kijelentkezés</Text>
            </TouchableOpacity>
          </>
        ) : (
          <>
            <TouchableOpacity 
              style={styles.authButton} 
              onPress={() => navigation.navigate('Login')}
            >
              <Text style={styles.authButtonText}>Bejelentkezés</Text>
            </TouchableOpacity>
            <TouchableOpacity 
              style={styles.authButton} 
              onPress={() => navigation.navigate('Registry')}
            >
              <Text style={styles.authButtonText}>Regisztráció</Text>
            </TouchableOpacity>
          </>
        )}
      </View>

        <Text style={styles.title}>Téma Választó</Text>
        <View style={styles.themeContainer}>
          {themes.map((t) => (
            <TouchableOpacity
              key={t.name}
              style={[
                styles.themeButton,
                theme === t.value && styles.selectedTheme,
              ]}
              onPress={() => updateTheme(t.value)}
            >
              <View style={[styles.colorBox, { backgroundColor: t.value }]} />
            </TouchableOpacity>
          ))}
        </View>
      </ImageBackground>
    </View>
  );
};

const getStyles = (theme) => {
  return StyleSheet.create({
    container: {
      flex: 1,
      justifyContent: 'center',
      alignItems: 'center',
      backgroundColor: theme,
    },
    authButtonsContainer: {
      alignItems: 'center',
      marginBottom: 20,
      width: "90%",
    },
    authButton: {
      backgroundColor: theme,
      padding: 15,
      borderRadius: 5,
      marginVertical: 10,
      width: '80%',
      alignItems: 'center',
    },
    authButtonText: {
      color: "#fff",
      fontSize: 16,
      fontWeight: 'bold',
    },
    title: {
      fontSize: 20,
      marginBottom: 20,
      color: '#fff',
      fontWeight: 'bold',
    },
    themeContainer: {
      width: '100%',
      flexDirection: 'row',
      flexWrap: 'wrap',
      justifyContent: 'center',
    },
    themeButton: {
      width: '20%',
      aspectRatio: 1,
      margin: 5,
      borderRadius: 5,
      justifyContent: 'center',
      alignItems: 'center',
    },
    selectedTheme: {
      borderColor: '#fff',
      borderWidth: 2,
      borderRadius: 5,
    },
    colorBox: {
      width: '100%',
      height: '100%',
      borderRadius: 2,
    },
    background: {
      flex: 1,
      width: '100%',
      height: '100%',
      justifyContent: 'center',
      alignItems: 'center',
    },
    backButton: {
      position: 'absolute',
      top: 45,
      left: 20,
      zIndex: 1,
    },
    settingsIcon: {
      fontSize: 50,
      color: theme,
    },
  });
};