from selenium import webdriver
from selenium.webdriver.common.by import By
from selenium.webdriver.support.ui import WebDriverWait
from selenium.webdriver.support import expected_conditions as EC
import pytest

@pytest.fixture
def driver():
    driver = webdriver.Chrome()
    yield driver
    driver.quit()
'''
def test_page_title(driver):
    driver.get("http://localhost:3000/")
    assert "Érettségizzünk" in driver.title, "Nem ez az oldal címe."

def test_element_presence(driver):
    driver.get("http://localhost:3000/")
    element = driver.find_element(By.TAG_NAME, "h1")
    assert element.text == "Érettségizzünk!", "Nincs ilyen h1 tag."

def test_navigation(driver):
    driver.get("http://localhost:3000/")
    div = driver.find_element(By.ID, "/selector")
    div.click()
    assert "http://localhost:3000/selector" == driver.current_url, "Nem ez az oldal url-je."    
'''
def test_function(driver):
    driver.get("http://localhost:3000/")
    selector_div = driver.find_element(By.ID, "/selector")
    selector_div.click()
    selector_button = driver.find_element(By.TAG_NAME, "button")
    selector_button.click()
    wait = WebDriverWait(driver, 3)
    last_button = wait.until(EC.presence_of_element_located((By.ID, "task15")))
    last_button.click()
    end_button = driver.find_element(By.ID, "taskDone")
    end_button.click()

    elements = driver.find_elements(By.XPATH, "//*[contains(text(), '❌')]")
    assert len(elements) > 0, "❌ element is not present on the page."
