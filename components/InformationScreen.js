import { useContext } from 'react';
import { ImageBackground, View, Text, StyleSheet, Linking } from 'react-native';
import { FontAwesome5 } from '@expo/vector-icons';
import { MaterialCommunityIcons } from '@expo/vector-icons';
import { ThemeContext } from './ThemeContext';

export default InformationScreen = () => {
  const openDiscord = () => {
    Linking.openURL('https://discord.gg/uR3uvaY5tp');
  };
  const { theme } = useContext(ThemeContext);
  const styles = getStyles(theme);

  return (
    <View style={styles.container}>
      <ImageBackground source={require('../background.jpg')} style={styles.background} resizeMode="cover" >
        <View style={styles.mainContainer}>
          <Text style={styles.title}>Miért válassz minket?</Text>
          <View style={styles.featuresContainer}>
            {/* Feature 1 */}
            <View style={styles.featureItem}>
              <FontAwesome5 name="book-open" size={32} color="#007bff" style={styles.icon} />
              <Text style={[styles.featureTitle, { color: '#007bff' }]}>Széles választék</Text>
              <Text style={styles.featureText}>Rengeteg feladat és segédanyag elérhető különböző tantárgyakból.</Text>
            </View>
            
            {/* Feature 2 */}
            <View style={styles.featureItem}>
              <FontAwesome5 name="users" size={32} color="#17a2b8" style={styles.icon} />
              <Text style={[styles.featureTitle, { color: '#17a2b8' }]}>Közösség</Text>
              <Text style={styles.featureText}>
                Csatlakozz más diákokhoz és osszd meg tapasztalataidat a{' '}
                <Text style={styles.discordLink} onPress={openDiscord}>
                  Discord
                </Text>{' '}
                szerverünkön.
              </Text>
            </View>
            
            {/* Feature 3 */}
            <View style={styles.featureItem}>
              <MaterialCommunityIcons name="chart-line" size={32} color="#28a745" style={styles.icon} />
              <Text style={[styles.featureTitle, { color: '#28a745' }]}>Statisztikák</Text>
              <Text style={styles.featureText}>Kövesd a fejlődésed és lásd, hol érdemes még gyakorolnod.</Text>
            </View>
          </View>
        </View>
      </ImageBackground>
    </View>
  );
};

const getStyles = (theme) => StyleSheet.create({
  container: {
    flex: 1,
  },
  mainContainer: {
    padding: 10,
    top: 40,
  },
  title: {
    fontSize: 24,
    fontWeight: 'bold',
    textAlign: 'center',
    marginBottom: 30,
    color: '#fff',
  },
  featuresContainer: {
    marginHorizontal: 10,
  },
  background: {
    flex: 1,
    width: '100%',
    height: '100%',
    justifyContent: 'center',
    alignItems: 'center',
  },
  featureItem: {
    backgroundColor: 'white',
    borderRadius: 10,
    padding: 20,
    marginBottom: 20,
    shadowColor: '#000',
    shadowOffset: {
      width: 0,
      height: 2,
    },
    shadowOpacity: 0.1,
    shadowRadius: 4,
    elevation: 3,
    alignItems: 'center',
  },
  icon: {
    marginBottom: 15,
  },
  featureTitle: {
    fontSize: 18,
    fontWeight: 'bold',
    marginBottom: 10,
  },
  featureText: {
    textAlign: 'center',
    color: '#6c757d',
    lineHeight: 20,
  },
  discordLink: {
    color: theme,
    fontWeight: 'bold',
  },
});