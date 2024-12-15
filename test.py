from selenium import webdriver
from selenium.webdriver.common.by import By
import pytest

@pytest.fixture
def driver():
    driver = webdriver.Chrome()
    yield driver
    driver.quit()

def test_page_title(driver):
    driver.get("http://localhost:3000/")
    assert "Érettségizzünk" in driver.title

def test_element_presence(driver):
    driver.get("http://localhost:3000/")
    element = driver.find_element(By.TAG_NAME, "h1")
    assert element.text == "Érettségizzünk!"

def test_navigation(driver):
    driver.get("http://localhost:3000/")
    div = driver.find_element(By.ID, "/selector")
    div.click()
    assert "http://localhost:3000/selector" == driver.current_url

