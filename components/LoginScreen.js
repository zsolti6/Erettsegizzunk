import  { useState, useContext } from "react";
import { Alert, ImageBackground, Switch, View, Text, TextInput, TouchableOpacity, StyleSheet, Image, ScrollView, ActivityIndicator } from "react-native";
import axios from "axios";
import sha256 from "crypto-js/sha256";
import { useNavigation } from "@react-navigation/native";
const BASE_URL = "https://erettsegizzunk.onrender.com";
import AsyncStorage from '@react-native-async-storage/async-storage';
import { ThemeContext } from './ThemeContext';

export default LoginPage = ({ handleLogin, handleGuestLogin }) => {
  const [passwordVisible, setPasswordVisible] = useState(false);
  const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");
  const [rememberMe, setRememberMe] = useState(false);
  const [loading, setLoading] = useState(false);
  const [errorMessage, setErrorMessage] = useState("");
  const navigation = useNavigation();
  const { theme } = useContext(ThemeContext);
  const styles = getStyles(theme);

  const handleLoginSubmit = async () => {
    setLoading(true);
    setErrorMessage("");
    
    try {
      const saltUrl = `${BASE_URL}/erettsegizzunk/Login/SaltRequest`;
      const saltResponse = await axios.post(saltUrl, JSON.stringify(username), {
        headers: {
          "Content-Type": "application/json",
        },
      });

      const salt = saltResponse.data;
      const tmpHash = sha256(password + salt.toString()).toString();
      const loginUrl = `${BASE_URL}/erettsegizzunk/Login`;
      const body = {
        loginName: username,
        tmpHash: tmpHash,
      };

      const loginResponse = await axios.post(loginUrl, body);
      if (loginResponse.status === 200) {
        const userData = loginResponse.data;
        if (rememberMe) {
          await AsyncStorage.setItem('userData', JSON.stringify(userData));
          await AsyncStorage.setItem('rememberMe', 'true');
        } else {
          await AsyncStorage.setItem('sessionUserData', JSON.stringify(userData));
        }
        await handleLogin(userData, rememberMe);
        navigation.replace("MainMenu");
      } else {
        setErrorMessage("Login failed");
      }
    } catch (error) {
      setErrorMessage(error.message);
    } finally {
      setLoading(false);
    }
  };

  return (
    <ScrollView contentContainerStyle={styles.container}>
    <ImageBackground source={require('../background.jpg')} style={styles.background} resizeMode="cover" >
      {loading && (
        <View style={styles.loadingOverlay}>
          <ActivityIndicator size="large" color="#0000ff" />
        </View>
      )}

      <View style={styles.content}>

        <View style={styles.card}>
          <Text style={styles.title}>Bejelentkezés</Text>

          {errorMessage ? (
            <View style={styles.errorContainer}>
              <Text style={styles.errorText}>{errorMessage}</Text>
            </View>
          ) : null}

          <View style={styles.formGroup}>
            <TextInput
              placeholder="Felhasználónév"
              style={styles.input}
              value={username}
              onChangeText={setUsername}
              autoCapitalize="none"
              required
            />
          </View>

          <View style={styles.formGroup}>
            <View style={styles.passwordContainer}>
              <TextInput
                placeholder="Jelszó"
                style={styles.passwordInput}
                secureTextEntry={!passwordVisible}
                value={password}
                onChangeText={setPassword}
                required
              />
              <TouchableOpacity 
                style={styles.toggleButton}
                onPress={() => setPasswordVisible(!passwordVisible)}
              >
                <Text style={styles.toggleButtonText}>
                  {passwordVisible ? "🙈" : "👁️"}
                </Text>
              </TouchableOpacity>
            </View>
          </View>

          <View style={styles.rememberMeContainer}>
            <Switch
              value={rememberMe}
              onValueChange={setRememberMe}
              trackColor={styles.rememberMeSwitch}
            />
            <Text style={styles.rememberMeText}>Emlékezz rám</Text>
          </View>

          <TouchableOpacity 
            style={styles.loginButton}
            onPress={handleLoginSubmit}
          >
            <Text style={styles.loginButtonText}>Belépés</Text>
          </TouchableOpacity>

          <View style={styles.linksContainer}>
            <TouchableOpacity onPress={() => navigation.navigate("ForgotPassword")}>
              <Text style={styles.linkText}>Elfelejtett jelszó</Text>
            </TouchableOpacity>
            <TouchableOpacity onPress={() => navigation.navigate("Registry")}>
              <Text style={styles.linkText}>Még nincs fiókod?</Text>
            </TouchableOpacity>
          </View>

          <TouchableOpacity 
            style={styles.guestButton}
            onPress={() => {
              handleGuestLogin();
              navigation.navigate("MainMenu");
            }}
          >
            <Text style={styles.guestButtonText}>Folytatás vendégként</Text>
          </TouchableOpacity>
        </View>
      </View>
    </ImageBackground>
    </ScrollView>
  );
};

const getStyles = (theme) => StyleSheet.create({
  container: {
    flexGrow: 1,
    backgroundColor: '#fff',
  },
  loadingOverlay: {
    ...StyleSheet.absoluteFillObject,
    backgroundColor: 'rgba(255, 255, 255, 0.7)',
    justifyContent: 'center',
    alignItems: 'center',
    zIndex: 1,
  },
  content: {
    flex: 1,
    justifyContent: 'center',
    alignContent: 'center',
  },
  image: {
    width: '100%',
    height: 200,
    resizeMode: 'cover',
  },
  card: {
    backgroundColor: '#fff',
    padding: 20,
    borderRadius: 10,
    shadowColor: '#000',
    shadowOffset: { width: 0, height: 2 },
    shadowOpacity: 0.1,
    shadowRadius: 6,
    elevation: 3,
    margin: 20,
  },
  title: {
    fontSize: 24,
    fontWeight: 'bold',
    textAlign: 'center',
    marginBottom: 20,
    color: '#333',
  },
  background: {
    flex: 1,
  },
  errorContainer: {
    backgroundColor: '#f8d7da',
    padding: 10,
    borderRadius: 5,
    marginBottom: 15,
  },
  errorText: {
    color: '#721c24',
    textAlign: 'center',
  },
  formGroup: {
    marginBottom: 15,
  },
  input: {
    borderWidth: 1,
    borderColor: '#ddd',
    borderRadius: 5,
    padding: 12,
    fontSize: 16,
  },
  passwordContainer: {
    flexDirection: 'row',
    alignItems: 'center',
    borderWidth: 1,
    borderColor: '#ddd',
    borderRadius: 5,
  },
  passwordInput: {
    flex: 1,
    padding: 12,
    fontSize: 16,
  },
  toggleButton: {
    padding: 10,
  },
  toggleButtonText: {
    fontSize: 18,
  },
  rememberMeContainer: {
    flexDirection: 'row',
    alignItems: 'center',
    marginBottom: 15,
  },
  rememberMeText: {
    marginLeft: 8,
    color: '#555',
  },
  rememberMeSwitch: {
    false: "#767577",
    true: theme,
  },
  loginButton: {
    backgroundColor: theme,
    padding: 15,
    borderRadius: 5,
    alignItems: 'center',
    marginBottom: 15,
  },
  loginButtonText: {
    color: '#fff',
    fontSize: 16,
    fontWeight: 'bold',
  },
  linksContainer: {
    flexDirection: 'row',
    justifyContent: 'space-between',
    marginBottom: 20,
  },
  linkText: {
    color: theme,
    textDecorationLine: 'underline',
  },
  guestButton: {
    backgroundColor: '#e0e0e0',
    padding: 15,
    borderRadius: 5,
    alignItems: 'center',
    marginBottom: 15,
  },
  guestButtonText: {
    color: '#333',
    fontSize: 16,
    fontWeight: 'bold',
  },
});