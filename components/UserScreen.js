import { useState, useEffect, useContext } from 'react';
import { ImageBackground, View, Text, TextInput, StyleSheet, TouchableOpacity, ScrollView, ActivityIndicator, Switch, } from 'react-native';
import axios from 'axios';
import sha256 from 'crypto-js/sha256';
import { useNavigation } from '@react-navigation/native';
import Modal from 'react-native-modal';
import Icon from 'react-native-vector-icons/MaterialIcons';
import AsyncStorage from '@react-native-async-storage/async-storage';
const BASE_URL = "https://erettsegizzunk.onrender.com";
import { ThemeContext } from './ThemeContext';

export default UserScreen = ({ user, setUser}) => {
  const [userData, setUserData] = useState({
    id: 0,
    name: 'string',
    email: 'string',
    permission: 0,
    newsletter: false,
    profilePicture: 'string',
    profilePicturePath: 'string',
    token: 'string',
  });
  const [passwordVisible, setPasswordVisible] = useState(false);
  const [changePassword, setChangePassword] = useState(false);
  const [loading, setLoading] = useState(false);
  const [showResetModal, setShowResetModal] = useState(false);
  const [messageModal, setMessageModal] = useState({
    show: false,
    type: '',
    message: '',
  });
  const { theme } = useContext(ThemeContext);
  const styles = getStyles(theme);
  const navigation = useNavigation();

  const [formData, setFormData] = useState({
    token: '',
    loginName: '',
    oldPassword: '',
    newPassword: '',
  });

  useEffect(() => {
    if (!user) {
      navigation.navigate('Login');
    }
  }, [navigation, user]);

  useEffect(() => {
    if (user) {
      setUserData({
        name: user.name || '',
        email: user.email || '',
        newsletter: user.newsletter || false,
        id: user.id || 0,
        permission: user.permission || 0,
        profilePicture: user.profilePicture || null,
        profilePicturePath: user.profilePicturePath || '',
        token: user.token || '',
      });

      setFormData(prev => ({
        ...prev,
        token: user.token || '',
        loginName: user.name || '',
        oldPassword: changePassword ? '' : prev.oldPassword,
        newPassword: changePassword ? '' : prev.newPassword
      }));
    }
  }, [user, changePassword]);

  const resetStatistics = async () => {
    const body = {
      userId: user.id,
      token: user.token,
    };

    try {
      await axios.request({
        method: 'DELETE',
        url: `${BASE_URL}/erettsegizzunk/UserStatistics/statisztika-reset`,
        data: body,
        headers: {
          'Content-Type': 'application/json',
        },
      });
      setMessageModal({
        show: true,
        type: 'success',
        message: 'A statisztika sikeresen visszaállítva.',
      });
    } catch (error) {
      setMessageModal({
        show: true,
        type: 'error',
        message:
          error.response?.data?.message ||
          'Hiba történt a statisztika visszaállítása során.',
      });
    } finally {
      setShowResetModal(false);
    }
  };

  const handleInputChange = (name, value) => {
    setUserData({
      ...userData,
      [name]: value,
    });
  };

  const handleSubmit = async () => {
    setLoading(true);

    if (changePassword) {
      try {
        const saltUrl = `${BASE_URL}/erettsegizzunk/Login/SaltRequest`;
        const saltResponse = await axios.post(
          saltUrl,
          JSON.stringify(userData.name),
          {
            headers: {
              'Content-Type': 'application/json',
            },
          }
        );

        const salt = saltResponse.data;
        const tmpHashOldPswd = sha256(
          formData.oldPassword + salt.toString()
        ).toString();
        const tmpHashNewPswd = sha256(
          formData.newPassword + salt.toString()
        ).toString();
        const updatedFormData = {
          token: userData.token,
          loginName: userData.name,
          oldPassword: tmpHashOldPswd,
          newPassword: tmpHashNewPswd,
        };

        await axios.post(
          `${BASE_URL}/erettsegizzunk/Password/jelszo-modositas`,
          updatedFormData
        );

        setMessageModal({
          show: true,
          type: 'success',
          message: 'Jelszó sikeresen megváltoztatva!',
        });
      } catch (error) {
        setMessageModal({
          show: true,
          type: 'error',
          message:
            error.response?.data?.message ||
            'Hiba történt a jelszó módosítása során.',
        });
        setLoading(false);
        return;
      }
    }

    try {
      const response = await axios.put(
        `${BASE_URL}/erettsegizzunk/User/sajat-felhasznalo-modosit`,
        userData
      );
      
      if (response.status === 200) {
        const rememberMe = await AsyncStorage.getItem('rememberMe');
        const storageKey = rememberMe === 'true' ? 'userData' : 'sessionUserData';
        const storedUserString = await AsyncStorage.getItem(storageKey);
        const storedUser = storedUserString ? JSON.parse(storedUserString) : {};
        const updatedUser = {
          ...storedUser,
          name: userData.name,
          email: userData.email,
          newsletter: userData.newsletter,
        };
        await AsyncStorage.setItem(storageKey, JSON.stringify(updatedUser));
        setChangePassword(false);
        setUser(updatedUser);
        setMessageModal({
          show: true,
          type: 'success',
          message: 'Felhasználói adatok sikeresen frissítve!',
        });
      }
    } catch (error) {
      setMessageModal({
        show: true,
        type: 'error',
        message:
          error.response?.data?.message ||
          'Hiba történt az adatok frissítése során.',
      });
    } finally {
      setLoading(false);
    }
  };

  return (
    <ImageBackground source={require('../background.jpg')} style={styles.background} resizeMode="cover" >
      <ScrollView style={styles.container}>
        <View style={styles.mainContainer}>
          <View style={styles.card}>
            <Text style={styles.title}>Adataim</Text>

            {/* Username */}
            <View style={styles.inputContainer}>
              <Text style={styles.label}>Felhasználónév</Text>
              <TextInput
                style={styles.input}
                maxLength={10}
                value={userData.name}
                onChangeText={(text) => handleInputChange('name', text)}
                placeholder="Felhasználónév"
              />
            </View>

            {/* Email */}
            <View style={styles.inputContainer}>
              <Text style={styles.label}>Email cím</Text>
              <TextInput
                style={styles.input}
                value={userData.email}
                onChangeText={(text) => handleInputChange('email', text)}
                placeholder="Email cím"
                keyboardType="email-address"
              />
            </View>

            {/* Password Change Section */}
            <>
              {/* Password Change Toggle */}
              <View style={styles.switchContainer}>
                <Text style={styles.switchLabel}>
                  Szeretnék jelszavat változtatni
                </Text>
                <Switch
                  value={changePassword}
                  onValueChange={() => {
                    setChangePassword(!changePassword);
                    setFormData({
                      ...formData,
                      oldPassword: '',
                      newPassword: '',
                    });
                    setPasswordVisible(false);
                  }}
                  trackColor={styles.switchButton}
                  thumbColor={styles.switchButtonThumb}
                />
              </View>

              {/* Old Password */}
              {changePassword && (
                <View style={styles.inputContainer}>
                  <Text style={styles.label}>Régi jelszó</Text>
                  <TextInput
                    style={styles.input}
                    secureTextEntry={!passwordVisible}
                    value={formData.oldPassword}
                    onChangeText={(text) =>
                      setFormData({ ...formData, oldPassword: text })
                    }
                    placeholder="Régi jelszó"
                  />
                </View>
              )}

              {/* New Password */}
              {changePassword && (
                <View style={styles.inputContainer}>
                  <Text style={styles.label}>Új jelszó</Text>
                  <View style={styles.passwordInputContainer}>
                    <TextInput
                      style={[styles.input, { flex: 1 }]}
                      secureTextEntry={!passwordVisible}
                      value={formData.newPassword}
                      onChangeText={(text) =>
                        setFormData({ ...formData, newPassword: text })
                      }
                      placeholder="Új jelszó"
                    />
                    <TouchableOpacity
                      style={styles.eyeIcon}
                      onPress={() => setPasswordVisible(!passwordVisible)}>
                      <Icon
                        name={passwordVisible ? 'visibility' : 'visibility-off'}
                        size={24}
                        color="#666"
                      />
                    </TouchableOpacity>
                  </View>
                </View>
              )}
            </>

            {/* Newsletter */}
            <View style={styles.switchContainer}>
              <Text style={styles.switchLabel}>Feliratkozom a hírlevélre</Text>
              <Switch
                trackColor={styles.switchButton}
                value={userData.newsletter}
                onValueChange={(value) => handleInputChange('newsletter', value)}
                thumbColor={styles.switchButtonThumb}
              />
            </View>

            {/* Save Button */}
            <TouchableOpacity
              style={[styles.button, styles.saveButton]}
              onPress={handleSubmit}
              disabled={loading}>
              {loading ? (
                <ActivityIndicator color="#fff" />
              ) : (
                <Text style={styles.buttonText}>Mentés</Text>
              )}
            </TouchableOpacity>

            {/* Reset Statistics Button */}
            <TouchableOpacity
              style={[styles.button, styles.resetButton]}
              onPress={() => setShowResetModal(true)}>
              <Text style={styles.buttonText}>Statisztika visszaállítása</Text>
            </TouchableOpacity>
          </View>

          {/* Reset Confirmation Modal */}
          <Modal isVisible={showResetModal}>
            <View style={styles.modalContent}>
              <Text style={styles.modalTitle}>Statisztika visszaállítása</Text>
              <Text style={styles.modalText}>
                Biztosan vissza szeretnéd állítani a statisztikádat? Ez a művelet
                nem vonható vissza.
              </Text>
              <View style={styles.modalButtons}>
                <TouchableOpacity
                  style={[styles.modalButton, styles.cancelButton]}
                  onPress={() => setShowResetModal(false)}>
                  <Text style={styles.modalButtonText}>Mégse</Text>
                </TouchableOpacity>
                <TouchableOpacity
                  style={[styles.modalButton, styles.confirmButton]}
                  onPress={resetStatistics}>
                  <Text style={styles.modalButtonText}>Visszaállítás</Text>
                </TouchableOpacity>
              </View>
            </View>
          </Modal>

          {/* Message Modal */}
          <Modal isVisible={messageModal.show}>
            <View
              style={[
                styles.messageModalContent,
                messageModal.type === 'error'
                  ? styles.errorModal
                  : styles.successModal,
              ]}>
              <Text style={styles.messageModalText}>{messageModal.message}</Text>
              <TouchableOpacity
                style={styles.messageModalButton}
                onPress={() =>
                  setMessageModal({ ...messageModal, show: false })
                }>
                <Text style={styles.messageModalButtonText}>OK</Text>
              </TouchableOpacity>
            </View>
          </Modal>
        </View>
      </ScrollView>
    </ImageBackground>
  );
};

const getStyles = (theme) => StyleSheet.create({
  container: {
    flex: 1,
  },
  mainContainer: {
    top: 45,
    padding: 10,
    height: '100%',
  },
  switchButton: {
    false: "#767577",
    true: theme,
  },
  switchButtonThumb: {
    false: "#767577",
    true: theme,
  },
  card: {
    backgroundColor: '#fff',
    borderRadius: 10,
    padding: 20,
    shadowColor: '#000',
    shadowOffset: { width: 0, height: 2 },
    shadowOpacity: 0.1,
    shadowRadius: 4,
    elevation: 3,
  },
  title: {
    fontSize: 24,
    fontWeight: 'bold',
    marginBottom: 20,
    textAlign: 'center',
    color: '#333',
  },
  inputContainer: {
    marginBottom: 16,
  },
  label: {
    marginBottom: 8,
    fontSize: 16,
    color: '#333',
  },
  input: {
    borderWidth: 1,
    borderColor: '#ddd',
    borderRadius: 6,
    padding: 12,
    fontSize: 16,
    backgroundColor: '#fff',
  },
  passwordInputContainer: {
    flexDirection: 'row',
    alignItems: 'center',
  },
  eyeIcon: {
    position: 'absolute',
    right: 10,
    padding: 10,
  },
  switchContainer: {
    flexDirection: 'row',
    justifyContent: 'space-between',
    alignItems: 'center',
    marginBottom: 16,
    paddingVertical: 8,
  },
  switchLabel: {
    fontSize: 16,
    color: '#333',
    flex: 1,
  },
  button: {
    borderRadius: 6,
    padding: 15,
    alignItems: 'center',
    justifyContent: 'center',
    marginBottom: 12,
  },
  saveButton: {
    backgroundColor: theme,
  },
  resetButton: {
    backgroundColor: '#dc3545',
  },
  buttonText: {
    color: '#fff',
    fontSize: 16,
    fontWeight: 'bold',
  },
  modalContent: {
    backgroundColor: 'white',
    padding: 20,
    borderRadius: 10,
  },
  modalTitle: {
    fontSize: 20,
    fontWeight: 'bold',
    marginBottom: 15,
    textAlign: 'center',
  },
  modalText: {
    fontSize: 16,
    marginBottom: 20,
    textAlign: 'center',
  },
  modalButtons: {
    flexDirection: 'row',
    justifyContent: 'space-between',
  },
  modalButton: {
    flex: 1,
    padding: 12,
    borderRadius: 6,
    alignItems: 'center',
    marginHorizontal: 5,
  },
  cancelButton: {
    backgroundColor: '#6c757d',
  },
  confirmButton: {
    backgroundColor: theme,
  },
  modalButtonText: {
    color: '#fff',
    fontWeight: 'bold',
  },
  messageModalContent: {
    backgroundColor: 'white',
    padding: 20,
    borderRadius: 10,
    alignItems: 'center',
  },
  background: {
    flex: 1,
  },
  errorModal: {
    borderTopWidth: 5,
    borderTopColor: '#dc3545',
  },
  successModal: {
    borderTopWidth: 5,
    borderTopColor: '#28a745',
  },
  messageModalText: {
    fontSize: 16,
    marginBottom: 20,
    textAlign: 'center',
  },
  messageModalButton: {
    padding: 10,
    width: '100%',
    backgroundColor: theme,
    borderRadius: 6,
    alignItems: 'center',
  },
  messageModalButtonText: {
    color: '#fff',
    fontWeight: 'bold',
  },
});