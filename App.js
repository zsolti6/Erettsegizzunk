import { useEffect, useState, useCallback } from 'react';
import { NavigationContainer } from '@react-navigation/native';
import { createStackNavigator } from '@react-navigation/stack';
import { ThemeProvider } from './components/ThemeContext';
import MainMenu from './components/MainMenu';
import SelectorScreen from './components/SelectorScreen';
import TestStatisticsScreen from './components/TestStatisticsScreen';
import SettingsScreen from './components/SettingsScreen';
import InformationScreen from './components/InformationScreen';
import LoginScreen from './components/LoginScreen';
import RegistryScreen from './components/RegistryScreen';
import TestScreen from './components/TestScreen';
import UserScreen from './components/UserScreen';
import CarouselScreen from './components/CarouselScreen';
import StatisticsScreen from './components/Statistics/StatisticsScreen';
import ForgotPasswordScreen from './components/ForgotPasswordScreen';
import * as ScreenOrientation from 'expo-screen-orientation';
import AsyncStorage from '@react-native-async-storage/async-storage';
import { ActivityIndicator, View, AppState, StyleSheet } from 'react-native';

const Stack = createStackNavigator();

export default App = () => {
  const [user, setUser] = useState(null);
  const [isGuest, setIsGuest] = useState(false);
  const [isLoading, setIsLoading] = useState(true);
  const [errorMessage, setErrorMessage] = useState("");

  const handleLogin = async (userData, remember) => {
    try {
      if (remember) {
        await AsyncStorage.setItem('rememberMe', 'true');
        await AsyncStorage.setItem('userData', JSON.stringify(userData));
      } else {
        await AsyncStorage.setItem('sessionUserData', JSON.stringify(userData));
        await AsyncStorage.setItem('rememberMe', 'false');
      }
      setUser(userData);
      setIsGuest(false);
      setErrorMessage("");
    } catch {
      setErrorMessage("Failed to save login data. Please try again.");
    }
  };

  const handleGuestLogin = async () => {
    try {
      const guestUser = {
        id: 'guest',
        username: 'guest',
        isGuest: true,
      };
      await AsyncStorage.setItem('isGuest', 'true');
      setUser(guestUser);
      setIsGuest(true);
      setErrorMessage("");
    } catch {
      setErrorMessage("Failed to set up guest session. Please try again.");
    }
  };

  const handleLogout = useCallback(async () => {
    setErrorMessage("");
    try {
      if (user?.token && !user?.isGuest) {
        try {
          await axios.post(
            `${BASE_URL}/erettsegizzunk/Logout`,
            user.token,
            { headers: { "Content-Type": "application/json" } }
          );
        } catch {
          setErrorMessage("Failed to notify server about logout. You may need to login again.");
        }
      }
      await AsyncStorage.multiRemove([
        'userData',
        'sessionUserData',
        'rememberMe',
        'isGuest'
      ]);
      
      setUser(null);
      setIsGuest(false);
    } catch {
      setErrorMessage("Failed to clear local session data. Please try again.");
    }
  }, [user]);

  useEffect(() => {
    const checkLoginStatus = async () => {
      try {
        const isGuest = await AsyncStorage.getItem('isGuest');
        if (isGuest === 'true') {
          handleGuestLogin();
          setIsLoading(false);
          return;
        }
        const rememberMe = await AsyncStorage.getItem('rememberMe');
        const storageKey = rememberMe === 'true' ? 'userData' : 'sessionUserData';
        const userDataString = await AsyncStorage.getItem(storageKey);

        if (userDataString) {
          const userData = JSON.parse(userDataString);
          setUser(userData);
          setIsGuest(false);
        }
      } catch {
        setErrorMessage("Failed to check login status. Please restart the app.");
      } finally {
        setIsLoading(false);
      }
    };

    checkLoginStatus();
    ScreenOrientation.lockAsync(ScreenOrientation.OrientationLock.PORTRAIT);
    
    return () => {
      ScreenOrientation.unlockAsync();
    };
  }, []);

  const checkActiveStatus = useCallback(async () => {
    if (user && user.token) {
      try {
        const response = await axios.post(
          `${BASE_URL}/erettsegizzunk/Logout/active`,
          user.token,
          { headers: { "Content-Type": "application/json" } }
        );
        
        if (response.data === false) { 
          await handleLogout();
        }
      } catch {
        setErrorMessage("Session verification failed. Please login again.");
      }
    }
  }, [user, handleLogout]);

  useEffect(() => {
    const subscription = AppState.addEventListener('change', (nextAppState) => {
      if (nextAppState === 'active') {
        checkActiveStatus();
      }
    });
    return () => {
      subscription.remove();
    };
  }, [checkActiveStatus]);

  if (isLoading) {
    return (
      <View style={styles.loadingContainer}>
        <ActivityIndicator size="large" />
      </View>
    );
  }

  return (
    <ThemeProvider>
        <NavigationContainer>
          <Stack.Navigator initialRouteName={user ? "MainMenu" : "Login"}>
            {user ? (
              <>
                <Stack.Screen name="MainMenu" options={{ headerShown: false }}>
                  {(props) => <MainMenu {...props} user={user} isGuest={isGuest} handleLogout={handleLogout} errorMessage={errorMessage} />}
                </Stack.Screen>

                <Stack.Screen name="Selector" options={{ headerShown: false }}>
                  {(props) => <SelectorScreen {...props} user={user} errorMessage={errorMessage} />}
                </Stack.Screen>

                <Stack.Screen name="TestStatistics" component={TestStatisticsScreen} options={{ headerShown: false }}>
                  {(props) => ( <CarouselScreen {...props} user={user} errorMessage={errorMessage} /> )}
                </Stack.Screen>

                <Stack.Screen name="Statistics" options={{ headerShown: false }}>
                  {(props) => <StatisticsScreen {...props} user={user} errorMessage={errorMessage} />}
                </Stack.Screen>

                <Stack.Screen name="Settings" options={{ headerShown: false }}>
                  {(props) => ( <SettingsScreen {...props} user={user} setUser={setUser} handleLogout={handleLogout} errorMessage={errorMessage} /> )}
                </Stack.Screen>

                <Stack.Screen name="Information" component={InformationScreen} options={{ headerShown: false }} />

                <Stack.Screen name="Test" options={{ headerShown: false }}>
                  {(props) => <TestScreen {...props} user={user} errorMessage={errorMessage} />}
                </Stack.Screen>

                <Stack.Screen name="CarouselScreen" options={{ headerShown: false }}>
                  {(props) => ( <CarouselScreen {...props} user={user} errorMessage={errorMessage} /> )}
                </Stack.Screen>

                <Stack.Screen name="Profile" options={{ headerShown: false }}>
                  {(props) => ( <UserScreen {...props} user={user} setUser={setUser} handleLogout={handleLogout} errorMessage={errorMessage} /> )}
                </Stack.Screen>

                <Stack.Screen name="Login" options={{ headerShown: false }}>
                  {(props) => ( <LoginScreen {...props} handleLogin={handleLogin} handleGuestLogin={handleGuestLogin} errorMessage={errorMessage} /> )}
                </Stack.Screen>

                <Stack.Screen name="Registry" component={RegistryScreen} options={{ headerShown: false }} />

                <Stack.Screen name="ForgotPassword" component={ForgotPasswordScreen} options={{ headerShown: false }} />
              </>
            ) : (
              <>
                <Stack.Screen name="Login" options={{ headerShown: false }}>
                  {(props) => ( <LoginScreen {...props} handleLogin={handleLogin} handleGuestLogin={handleGuestLogin} errorMessage={errorMessage} /> )}
                </Stack.Screen>

                <Stack.Screen name="ForgotPassword" component={ForgotPasswordScreen} options={{ headerShown: false }} />

                <Stack.Screen name="Registry" component={RegistryScreen} options={{ headerShown: false }} />
              </>
            )}
          </Stack.Navigator>
        </NavigationContainer>
    </ThemeProvider>
  );
};

const styles = StyleSheet.create({
  loadingContainer: {
    flex: 1,
    justifyContent: 'center',
    alignItems: 'center',
    backgroundColor: '#ffffff',
  },
});