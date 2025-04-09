import React, { useState } from "react";
import { ImageBackground, View, Text, TextInput, TouchableOpacity, StyleSheet, ActivityIndicator, ScrollView } from "react-native";
import axios from "axios";
const BASE_URL = "https://erettsegizzunk.onrender.com";
import { useNavigation } from "@react-navigation/native";
import { ThemeContext } from './ThemeContext';
import { useContext } from 'react';

export default ForgotPasswordScreen = () => {
  const [email, setEmail] = useState("");
  const [message, setMessage] = useState({ show: false, type: "", text: "" });
  const [loading, setLoading] = useState(false);
  const navigation = useNavigation();
  const { theme } = useContext(ThemeContext);
  const styles = getStyles(theme);

  const handlePasswordReset = async () => {
    setLoading(true);
    setMessage({ show: false, type: "", text: "" });

    try {
      const response = await axios.post(
        `${BASE_URL}/erettsegizzunk/Password/elfelejtett-jelszo-keres`,
        JSON.stringify(email),
        {
          headers: {
            "Content-Type": "application/json",
          },
        }
      );

      if (response.status === 200) {
        setMessage({
          show: true,
          type: "success",
          text: "Sikeres jelszó visszaállítási kérelem. Ellenőrizze az email fiókját!",
        });
      } else {
        setMessage({
          show: true,
          type: "error",
          text: "Hiba történt a kérés feldolgozásakor. Próbálja újra később.",
        });
      }
    } catch (error) {
      console.error("Password reset request failed", error);
      setMessage({
        show: true,
        type: "error",
        text: "Nem sikerült elküldeni a kérést. Ellenőrizze az email címet és próbálja újra.",
      });
    } finally {
      setLoading(false);
    }
  };

  return (
    <ScrollView contentContainerStyle={styles.container}>
      <ImageBackground source={require('../background.jpg')} style={styles.background} resizeMode="cover" >
        <View style={styles.card}>
          <Text style={styles.title}>Jelszó visszaállítása</Text>

          {message.show && (
            <View style={[
              styles.messageContainer,
              message.type === "success" ? styles.successMessage : styles.errorMessage
            ]}>
              <Text style={styles.messageText}>{message.text}</Text>
            </View>
          )}

          <View style={styles.formGroup}>
            <TextInput
              placeholder="Email cím"
              style={styles.input}
              value={email}
              onChangeText={setEmail}
              keyboardType="email-address"
              autoCapitalize="none"
              required
            />
          </View>

          <TouchableOpacity
            style={styles.submitButton}
            onPress={handlePasswordReset}
            disabled={loading}
          >
            {loading ? (
              <ActivityIndicator color="#fff" />
            ) : (
              <Text style={styles.submitButtonText}>Jelszó visszaállítási link küldése</Text>
            )}
          </TouchableOpacity>

          <View style={styles.linksContainer}>
            <TouchableOpacity onPress={() => navigation.navigate("Login")}>
              <Text style={styles.linkText}>Vissza a bejelentkezéshez</Text>
            </TouchableOpacity>
            <TouchableOpacity onPress={() => navigation.navigate("Registry")}>
              <Text style={styles.linkText}>Még nincs fiókod?</Text>
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
    justifyContent: 'center',
    alignContent: 'center',
    backgroundColor: '#f5f5f5',
  },
  card: {
    backgroundColor: '#fff',
    borderRadius: 10,
    padding: 20,
    shadowColor: '#000',
    shadowOffset: { width: 0, height: 2 },
    shadowOpacity: 0.1,
    shadowRadius: 6,
    elevation: 3,
  },
  title: {
    fontSize: 24,
    fontWeight: 'bold',
    textAlign: 'center',
    marginBottom: 20,
    color: '#333',
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
  submitButton: {
    backgroundColor: theme,
    padding: 15,
    borderRadius: 5,
    alignItems: 'center',
    marginBottom: 15,
  },
  background: {
    flex: 1,
    padding: 20,
    justifyContent: 'center',
    alignContent: 'center',
  },
  submitButtonText: {
    color: '#fff',
    fontSize: 16,
    fontWeight: 'bold',
  },
  linksContainer: {
    flexDirection: 'row',
    justifyContent: 'space-between',
    marginTop: 10,
  },
  linkText: {
    color: theme,
    textDecorationLine: 'underline',
  },
  messageContainer: {
    padding: 15,
    borderRadius: 5,
    marginBottom: 15,
  },
  successMessage: {
    backgroundColor: '#d4edda',
  },
  errorMessage: {
    backgroundColor: '#f8d7da',
  },
  messageText: {
    color: '#155724',
    textAlign: 'center',
  },
});