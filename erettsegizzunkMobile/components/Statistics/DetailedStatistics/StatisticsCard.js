import React, { useState, useContext } from 'react';
import { View, Text, StyleSheet, TouchableOpacity, Image, Modal, Dimensions, } from 'react-native';
import { PieChart } from 'react-native-svg-charts';
import { MaterialIcons, FontAwesome } from '@expo/vector-icons';
const IMG_URL = 'https://res.cloudinary.com/drpkpopsq/image/upload/v1741078235/';
const COLORS = ['#00FF00', '#FF0000'];
import { ThemeContext } from '../../ThemeContext';

export const StatisticsCard = React.memo(
  ({ item, isExpanded, onToggleExpand }) => {
    const correct = item.joRossz[0];
    const incorrect = item.joRossz[1];
    const total = correct + incorrect;
    const percentage = total > 0 ? ((correct / total) * 100).toFixed(1) : 0;
    const { theme } = useContext(ThemeContext);
    const styles = getStyles(theme);
    const [modalVisible, setModalVisible] = useState(false);
    const chartData = React.useMemo(
      () => [
        { value: correct, svg: { fill: COLORS[0] }, key: 'correct' },
        { value: incorrect, svg: { fill: COLORS[1] }, key: 'incorrect' },
      ],
      [correct, incorrect]
    );

    const formattedDate = new Date(item.utolsoKitoltesDatum).toLocaleDateString(
      'hu-HU'
    );
    const themesList =
      item.task.themes.map((x) => x.name).join(', ') || 'Nincs téma';

    return (
      <View style={styles.cardContainer}>
        {/* Card Header */}
        <TouchableOpacity
          style={styles.cardHeader}
          onPress={onToggleExpand}
          activeOpacity={0.8}>
          <Text
            style={styles.headerText}
            numberOfLines={1}
            ellipsizeMode="tail">
            {item.task.description}
          </Text>

          <View style={styles.headerStats}>
            <Text style={styles.statText}>
              <Text style={styles.boldText}>Sikeres:</Text> {correct}
            </Text>
            <Text style={styles.statText}>
              <Text style={styles.boldText}>Sikertelen:</Text> {incorrect}
            </Text>
          </View>

          <MaterialIcons
            name={isExpanded ? 'keyboard-arrow-up' : 'keyboard-arrow-down'}
            size={24}
            color="white"
          />
        </TouchableOpacity>

        {/* Expanded Content */}
        {isExpanded && (
          <View style={styles.cardBody}>
            <View style={styles.contentRow}>
              {/* Task Description Column */}
              <View style={styles.descriptionColumn}>
                <View style={styles.section}>
                  <Text style={styles.sectionTitle}>Feladat leírása:</Text>
                  <Text style={styles.sectionText}>{item.task.description}</Text>
                </View>

                <View style={styles.section}>
                  <Text style={styles.sectionTitle}>Feladat szövege:</Text>
                  <Text style={styles.sectionText}>{item.task.text}</Text>
                </View>
              </View>

              {/* Metadata Row for Témák and Tantárgy */}
              <View style={styles.metadataRow}>
                <View style={styles.section}>
                  <Text style={styles.sectionTitle}>Témák:</Text>
                  <Text style={styles.sectionTextCenter}>{themesList}</Text>
                </View>

                <View style={styles.section}>
                  <Text style={styles.sectionTitle}>Tantárgy:</Text>
                  <Text style={styles.sectionTextCenter}>{item.task.subject.name}</Text>
                </View>
              </View>

              {/* Metadata Row for Utolsó kitöltés and Eredmény */}
              <View style={styles.metadataRow}>
                <View style={styles.section}>
                  <Text style={styles.sectionTitle}>Utolsó kitöltés:</Text>
                  <Text style={styles.sectionTextCenter}>{formattedDate}</Text>
                </View>

                <View style={styles.section}>
                  <Text style={styles.sectionTitle}>Eredmény:</Text>
                  <Text style={styles.sectionTextCenter}>
                    {item.utolsoSikeres ? '✅' : '❌'}
                  </Text>
                </View>
              </View>

              {/* Chart Column */}
              <View style={styles.chartSection}>
                <PieChart style={styles.pieChart}
                  data={chartData}
                  innerRadius={30}
                  outerRadius={40}
                  padAngle={0}
                />
                <Text style={styles.percentageText}>{percentage}%</Text>
              </View>

              {/* Image Icon Column */}
              {item.task.picName && (
                <TouchableOpacity
                  style={styles.imageButton}
                  onPress={() => setModalVisible(true)}>
                  <FontAwesome name="image" size={24} color="#4e54c8" />
                  <Text style={styles.imageButtonText}>Kép megtekintése</Text>
                </TouchableOpacity>
              )}
            </View>
          </View>
        )}

        {/* Image Modal */}
        <Modal
          animationType="fade"
          transparent={true}
          visible={modalVisible}
          onRequestClose={() => setModalVisible(false)}>
          <View style={styles.modalOverlay}>
            <View style={styles.modalContainer}>
              <View style={styles.modalHeader}>
                <Text style={styles.modalTitle}>Feladat kép</Text>
                <TouchableOpacity onPress={() => setModalVisible(false)}>
                  <MaterialIcons name="close" size={24} color="black" />
                </TouchableOpacity>
              </View>
              <Image
                style={styles.modalImage}
                source={{ uri: `${IMG_URL}${item.task.picName}` }}
                resizeMode="contain"
              />
            </View>
          </View>
        </Modal>
      </View>
    );
  },
  (prevProps, nextProps) => {
    return (
      prevProps.item.id === nextProps.item.id &&
      prevProps.isExpanded === nextProps.isExpanded
    );
  }
);

const getStyles = (theme) =>
  StyleSheet.create({
    cardContainer: {
      backgroundColor: theme,
      borderRadius: 8,
      marginBottom: 12,
      overflow: 'hidden',
      elevation: 2,
      shadowColor: '#000',
      shadowOffset: { width: 0, height: 1 },
      shadowOpacity: 0.2,
      shadowRadius: 1,
    },
    cardHeader: {
      flexDirection: 'row',
      justifyContent: 'space-between',
      alignItems: 'center',
      padding: 16,
      backgroundColor: theme,
    },
    headerText: {
      color: 'white',
      fontSize: 16,
      fontWeight: '500',
      flex: 1,
    },
    headerStats: {
      flexDirection: 'row',
    },
    statText: {
      color: 'white',
      marginLeft: 12,
      fontSize: 14,
    },
    boldText: {
      fontWeight: 'bold',
    },
    cardBody: {
      padding: 16,
      backgroundColor: theme,
    },
    contentRow: {
      flexDirection: 'column',
    },
    fullWidthSection: {
      width: '100%',
      marginBottom: 16,
    },
    sectionTitle: {
      fontWeight: 'bold',
      fontSize: 14,
      color: 'white',
      marginBottom: 4,
    },
    sectionText: {
      fontSize: 14,
      color: 'white',
      lineHeight: 20,
    },
    metadataRow: {
      flexDirection: 'row',
      justifyContent: 'space-between',
      marginBottom: 16,
    },
    metadataColumn: {
      width: '48%',
    },
    sectionTextCenter: {
      fontSize: 14,
      color: 'white',
      textAlign: 'left',
    },
    chartSection: {
      alignItems: 'center',
      marginBottom: 16,
      paddingVertical: 8,
    },
    chartContainer: {
      flexDirection: 'row',
      alignItems: 'center',
    },
    pieChart: {
      height: 100, 
      width: 100,
    },
    percentageText: {
      fontSize: 18,
      fontWeight: 'bold',
      color: 'white',
    },
    imageButtonContainer: {
      width: '100%',
      alignItems: 'center',
      marginTop: 8,
    },
    imageButton: {
      flexDirection: 'row',
      alignItems: 'center',
      padding: 10,
      borderRadius: 6,
      backgroundColor: '#f5f5f5',
    },
    imageButtonText: {
      marginLeft: 8,
      fontSize: 14,
      color: theme || '#4e54c8',
      fontWeight: '500',
    },
    modalOverlay: {
      flex: 1,
      backgroundColor: 'rgba(0,0,0,0.5)',
      justifyContent: 'center',
      alignItems: 'center',
    },
    modalContainer: {
      width: Dimensions.get('window').width * 0.9,
      backgroundColor: 'white',
      borderRadius: 8,
      overflow: 'hidden',
    },
    modalHeader: {
      flexDirection: 'row',
      justifyContent: 'space-between',
      alignItems: 'center',
      padding: 16,
      borderBottomWidth: 1,
      borderBottomColor: '#eee',
    },
    modalTitle: {
      fontSize: 18,
      fontWeight: 'bold',
    },
    modalImage: {
      width: '100%',
      height: Dimensions.get('window').height * 0.7,
    },
  });