import { useState, useEffect } from "react";
import { ImageBackground, View, Text, TextInput, TouchableOpacity, StyleSheet, Image, ScrollView, ActivityIndicator, Alert } from "react-native";
import axios from "axios";
import sha256 from "crypto-js/sha256";
import { useNavigation } from "@react-navigation/native";
const BASE_URL = "https://erettsegizzunk.onrender.com";
import { useContext } from 'react';
import { ThemeContext } from './ThemeContext';

export const GenerateSalt = (SaltLength) => {
  const karakterek = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
  let salt = "";
  for (let i = 0; i < SaltLength; i++) {
    const randomIndex = Math.floor(Math.random() * karakterek.length);
    salt += karakterek[randomIndex];
  }
  return salt;
}

function GenerateRandomPassword(length = 16) {
  const lowerCaseChars = "abcdefghijklmnopqrstuvwxyz";
  const upperCaseChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
  const digits = "0123456789";
  const specialChars = "!@#$%^&*()_+-=[]{}|;:,.<>?/";
  const allChars = lowerCaseChars + upperCaseChars + digits + specialChars;
  let password = "";
  password += lowerCaseChars[Math.floor(Math.random() * lowerCaseChars.length)];
  password += upperCaseChars[Math.floor(Math.random() * upperCaseChars.length)];
  password += digits[Math.floor(Math.random() * digits.length)];
  password += specialChars[Math.floor(Math.random() * specialChars.length)];
  for (let i = password.length; i < length; i++) {
    password += allChars[Math.floor(Math.random() * allChars.length)];
  }
  password = password.split('').sort(() => Math.random() - 0.5).join('');
  return password;
}

export default RegisterScreen = ({ user }) => {
  const [formData, setFormData] = useState({
    loginName: "",
    password: "",
    confirmPassword: "",
    email: ""
  });
  const [error, setError] = useState("");
  const [passwordVisible, setPasswordVisible] = useState(false);
  const [loading, setLoading] = useState(false);
  const navigation = useNavigation();
  const { theme } = useContext(ThemeContext);
  const styles = getStyles(theme);
  
  useEffect(() => {
    if (user != null) {
      navigation.navigate("MainMenu");
    }
  }, [navigation, user]);

  const togglePasswordVisibility = () => {
    setPasswordVisible(!passwordVisible);
  };

  const handleChange = (name, value) => {
    setFormData({ ...formData, [name]: value });
  };

  const handleGeneratePassword = () => {
    const newPassword = GenerateRandomPassword();
    setFormData({ ...formData, password: newPassword, confirmPassword: newPassword });
  };

  const validateForm = () => {
    if (!formData.loginName) return "Kérjük, adja meg a felhasználónevet!";
    if (!formData.email) return "Kérjük, adja meg az email címét!";
    if (!formData.password) return "Kérjük, adjon meg egy jelszót!";
    if (formData.password !== formData.confirmPassword) return "A jelszavak nem egyeznek!";
    return "";
  };

  const handleRegister = async () => {
    const validationError = validateForm();
    if (validationError) {
      setError(validationError);
      return;
    }
    
    setError("");
    setLoading(true);
    
    try {
      const salt = GenerateSalt(64);
      const requestData = {
        loginName: formData.loginName,
        hash: sha256(formData.password + salt).toString(),
        salt: salt,
        permissionId: 1,
        active: true,
        email: formData.email,
        profilePicturePath: "default.jpg"
      };

      const response = await axios.post(
        `${BASE_URL}/erettsegizzunk/Registry/regisztracio`,
        requestData,
        {
          headers: {
            'Content-Type': 'application/json',
            'Accept': 'application/json'
          }
        }
      );

      if (response.status === 200) {
        Alert.alert(
          "Sikeres regisztráció",
          "Most már bejelentkezhet a fiókjába",
          [
            { text: "OK", onPress: () => navigation.navigate("Login") }
          ]
        );
      }
    } catch (error) {
      let errorMessage = 'Hiba történt a regisztráció során';

      if (error.response) {
        errorMessage = error.response.data.message || JSON.stringify(error.response.data);
      } else if (error.request) {
        errorMessage = "Nem érkezett válasz a szervertől";
      } else {
        errorMessage = error.message;
      }

      setError(errorMessage);
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
          <Text style={styles.title}>Fiók létrehozása</Text>
          
          {error ? (
            <View style={styles.errorContainer}>
              <Text style={styles.errorText}>{error}</Text>
            </View>
          ) : null }
          
          <View style={styles.formGroup}>
            <TextInput
              placeholder="Felhasználónév"
              style={styles.input}
              value={formData.loginName}
              onChangeText={(text) => handleChange("loginName", text)}
              autoCapitalize="none"
              maxLength={10}
            />
          </View>
          
          <View style={styles.formGroup}>
            <TextInput
              placeholder="Email cím"
              style={styles.input}
              value={formData.email}
              onChangeText={(text) => handleChange("email", text)}
              keyboardType="email-address"
              autoCapitalize="none"
            />
          </View>
          
          <View style={styles.formGroup}>
            <View style={styles.passwordContainer}>
              <TextInput
                placeholder="Jelszó"
                style={styles.passwordInput}
                secureTextEntry={!passwordVisible}
                value={formData.password}
                onChangeText={(text) => handleChange("password", text)}
              />
              <TouchableOpacity 
                style={styles.toggleButton}
                onPress={togglePasswordVisibility}
              >
                <Text style={styles.toggleButtonText}>
                  {passwordVisible ? "🙈" : "👁️"}
                </Text>
              </TouchableOpacity>
              <TouchableOpacity 
                style={styles.generateButton}
                onPress={handleGeneratePassword}
              >
                <Text style={styles.toggleButtonText}>🎲</Text>
              </TouchableOpacity>
            </View>
          </View>
          
          <View style={styles.formGroup}>
            <TextInput
              placeholder="Jelszó megerősítése"
              style={styles.input}
              secureTextEntry={true}
              value={formData.confirmPassword}
              onChangeText={(text) => handleChange("confirmPassword", text)}
            />
          </View>
          
          <TouchableOpacity 
            style={styles.registerButton}
            onPress={handleRegister}
          >
            <Text style={styles.registerButtonText}>Regisztrálás</Text>
          </TouchableOpacity>
          
          <View style={styles.linksContainer}>
            <TouchableOpacity onPress={() => navigation.navigate("Login")}>
              <Text style={styles.linkText}>Van már fiókod? Jelentkezz be!</Text>
            </TouchableOpacity>
          </View>
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
  background: {
    flex: 1,
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
  generateButton: {
    padding: 10,
  },
  toggleButtonText: {
    fontSize: 18,
  },
  registerButton: {
    backgroundColor: theme,
    padding: 15,
    borderRadius: 5,
    alignItems: 'center',
    marginBottom: 15,
  },
  registerButtonText: {
    color: '#fff',
    fontSize: 16,
    fontWeight: 'bold',
  },
  linksContainer: {
    alignItems: 'center',
    marginBottom: 20,
  },
  linkText: {
    color: theme,
    textDecorationLine: 'underline',
  },
});