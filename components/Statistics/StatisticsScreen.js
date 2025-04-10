import React, { useEffect, useState, useContext } from "react";
import { ImageBackground, View, Text, StyleSheet, ActivityIndicator, TouchableOpacity, ScrollView, Alert } from "react-native";
import axios from "axios";
const BASE_URL = "https://erettsegizzunk.onrender.com";
import { StatisticsPieChart } from "./StatisticsPieChart";
import { LineGraph } from "./LineGraph";
import { DetailedStatistics } from "./DetailedStatistics";
import { FeatureCards } from "./FeatureCards";
import { Card } from "react-native-paper";
import { useNavigation } from "@react-navigation/native";
import { ThemeContext } from '../ThemeContext';

export default StatisticsComponent = ({ user, setErrorMessage }) => {
  const [loading, setLoading] = useState(true);
  const [userStats, setUserStats] = useState([]);
  const [fillingByDate, setFillingByDate] = useState([]);
  const navigation = useNavigation();
  const { theme } = useContext(ThemeContext);
  const styles = getStyles(theme);

  useEffect(() => {
    if (user.id != "guest") {
      const fetchInitialData = async () => {
        try {
          setLoading(true);

          const [statsResponse, fillingResponse] = await Promise.all([
            axios.post(`${BASE_URL}/erettsegizzunk/UserStatistics/get-taskFilloutCount`, {
              userId: user.id,
              token: user.token,
            }),
            axios.post(`${BASE_URL}/erettsegizzunk/UserStatistics/get-filling-byDate`, {
              userId: user.id,
              token: user.token,
            }),
          ]);

          const statsData = statsResponse.data;
          setUserStats(Object.entries(statsData).map(([name, count]) => ({ name, count })));
          setFillingByDate(Object.entries(fillingResponse.data).map(([date, value]) => ({ date, value })));
        } catch (error) {
          Alert.alert(
            "Hiba",
            "Hiba történt az adatok betöltése során. Kérjük, próbálja újra később.",
            [{ text: "OK" }]
          );
        } finally {
          setLoading(false);
        }
      };

      fetchInitialData();
    }
  }, [user]);

  if (user.id == "guest") {
    return (
      <View style={styles.container}>
        <ImageBackground source={require('../../background.jpg')} style={styles.background} resizeMode="cover" >
          <View style={styles.centeredContent}>
            <Text style={styles.title}>Bejelentkezés szükséges</Text>
            <Text style={styles.subtitle}>
              Jelentkezz be a statisztika funkció használatához.
            </Text>
            <TouchableOpacity
              style={styles.button}
              onPress={() => navigation.navigate('Login')}
            >
              <Text style={styles.buttonText}>Bejelentkezés</Text>
            </TouchableOpacity>
            <View style={styles.featureCardsContainer}>
              <FeatureCards />
            </View>
          </View>
        </ImageBackground>
      </View>
    );
  }

  if (loading) {
    return (
      <View style={styles.loadingContainer}>
        <ActivityIndicator size="large" color="#8884d8" />
      </View>
    );
  }

  if (userStats.length === 0) {
    return (
      <View style={styles.container}>
        <ImageBackground source={require('../../background.jpg')} style={styles.background} resizeMode="cover" >
          <View style={styles.centeredContent}>
            <Text style={styles.title}>Nincsenek elérhető statisztikák</Text>
            <Text style={styles.subtitle}>
              Úgy tűnik, hogy még nem fejeztél be egyetlen feladatot sem. Kezdd el a gyakorlást, hogy megtekinthesd a statisztikáidat!
            </Text>
            <TouchableOpacity
              style={styles.button}
              onPress={() => navigation.navigate('SelectorScreen')}
            >
              <Text style={styles.buttonText}>Kezdj el gyakorolni</Text>
            </TouchableOpacity>
          </View>
        </ImageBackground>
      </View>
    );
  }

  return (
    <View style={styles.fullContainer}>
      <ImageBackground source={require('../../background.jpg')} style={styles.background} resizeMode="cover" >
        <ScrollView style={styles.scrollContainer}>
            <View style={styles.mainContainer}>
              <View style={styles.chartsContainer}>
                //Under Construction
              </View>

              <Card style={styles.detailedStatsCard}>
                <Card.Content>
                  <DetailedStatistics user={user} setErrorMessage={setErrorMessage} />
                </Card.Content>
              </Card>
            </View>
        </ScrollView>
      </ImageBackground>
    </View>
  );
};

const getStyles = (theme) => 
  StyleSheet.create({
    fullContainer: {
      flex: 1,
    },
    container: {
      flex: 1,
      justifyContent: 'center',
    },
    scrollContainer: {
      flex: 1,
    },
    mainContainer: {
      flex: 1,
      padding: 8,
      top: 10,
    },
    centeredContent: {
      justifyContent: 'center',
      alignItems: 'center',
      top: 25,
      padding: 20,
    },
    title: {
      fontSize: 24,
      fontWeight: 'bold',
      color: 'white',
      marginBottom: 16,
      textAlign: 'center',
    },
    subtitle: {
      fontSize: 16,
      color: 'white',
      marginBottom: 24,
      textAlign: 'center',
    },
    button: {
      backgroundColor: theme,
      paddingHorizontal: 32,
      paddingVertical: 12,
      borderRadius: 8,
      marginBottom: 24,
      alignSelf: 'center',
    },
    buttonText: {
      color: 'white',
      fontSize: 16,
      fontWeight: 'bold',
    },
    loadingContainer: {
      flex: 1,
      justifyContent: 'center',
      backgroundColor: theme,
    },
    background: {
      flex: 1,
      width: '100%',
      height: '100%',
    },
    featureCardsContainer: {
      width: '100%',
      marginTop: 30,
    },
    chartsContainer: {
      marginBottom: 16,
    },
    detailedStatsCard: {
      flex: 1,
    }
  });