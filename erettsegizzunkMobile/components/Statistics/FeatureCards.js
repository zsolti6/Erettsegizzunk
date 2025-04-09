import { View, Text, StyleSheet } from 'react-native';
import { MaterialCommunityIcons } from '@expo/vector-icons';
import { Card } from 'react-native-paper';

export const FeatureCards = () => {
  const features = [
    {
      icon: 'chart-pie',
      title: 'Statisztikák',
      description: 'Kövesd a feladatok megoldásának eredményeit'
    },
    {
      icon: 'format-list-checks',
      title: 'Feladatok',
      description: 'Gyakorolj különböző témakörökben'
    },
    {
      icon: 'calendar',
      title: 'Idővonal',
      description: 'Nézd meg a kitöltéseid időrendben'
    }
  ];

  return (
    <View style={styles.container}>
      {features.map((feature, index) => (
        <Card key={index} style={styles.card}>
          <Card.Content style={styles.cardContent}>
            <MaterialCommunityIcons 
              name={feature.icon} 
              size={36} 
              color="white" 
              style={styles.icon} 
            />
            <Text style={styles.title}>{feature.title}</Text>
            <Text style={styles.description}>{feature.description}</Text>
          </Card.Content>
        </Card>
      ))}
    </View>
  );
};

const styles = StyleSheet.create({
  container: {
    flexDirection: 'row',
    flexWrap: 'wrap',
    justifyContent: 'center',
    padding: 8,
  },
  card: {
    width: '45%',
    minWidth: 120,
    margin: 8,
    backgroundColor: '#2c3e50',
    borderRadius: 8,
    elevation: 4,
  },
  cardContent: {
    alignItems: 'center',
    padding: 16,
  },
  icon: {
    marginBottom: 12,
  },
  title: {
    color: 'white',
    fontSize: 16,
    fontWeight: 'bold',
    marginBottom: 8,
    textAlign: 'center',
  },
  description: {
    color: 'white',
    fontSize: 14,
    textAlign: 'center',
  },
});