from selenium import webdriver
from selenium.webdriver.common.by import By
from selenium.webdriver.support.ui import WebDriverWait
from selenium.common.exceptions import NoSuchElementException
from selenium.webdriver.support import expected_conditions as EC
import pytest

@pytest.fixture
def driver():
    driver = webdriver.Chrome()
    yield driver
    driver.quit()

def test_page_title(driver):
    driver.get("https://erettsegizzunk.web.app/")
    assert "Érettségizzünk" in driver.title, "Nem ez az oldal címe."

def test_element_presence(driver):
    driver.get("https://erettsegizzunk.web.app/")
    element = driver.find_element(By.TAG_NAME, "h1")
    assert element.text == "Üdvözöllek az Érettségi Gyakorló Oldalon!", "Nincs ilyen h1 tag."

def test_navigation(driver):
    driver.get("https://erettsegizzunk.web.app/")
    div = driver.find_element(By.XPATH, "//*[text()='Új feladatlap']")
    div.click()
    assert "https://erettsegizzunk.web.app/feladat-valasztas" == driver.current_url, "Nem ez az oldal url-je."

def test_function(driver):
    driver.get("https://erettsegizzunk.web.app/")
    driver.maximize_window()
    selector_div = driver.find_element(By.XPATH, "//*[text()='Új feladatlap']")
    selector_div.click()
    wait = WebDriverWait(driver, 4)
    selector_button = driver.find_element(By.XPATH, "//*[text()='Feladatlap megkezdése']")
    selector_button.click()
    wait = WebDriverWait(driver, 3)
    for _ in range(14):
        tasks_button = wait.until(EC.presence_of_element_located((By.XPATH, "//*[text()='Következő feladat']")))
        tasks_button.click()
    end_button = driver.find_element(By.XPATH, "//*[text()='Feladatok leadása']")
    end_button.click()
    elements = driver.find_elements(By.XPATH, "//*[contains(text(), '❌')]")
    assert len(elements) > 0, "❌ element is not present on the page."

def test_login(driver):
    driver.get("https://erettsegizzunk.web.app/")
    driver.maximize_window()
    selector_div = driver.find_element(By.XPATH, "//*[text()='Bejelentkezés']")
    selector_div.click()
    wait = WebDriverWait(driver, 5)
    login_name = driver.find_element(By.XPATH, "//input[@placeholder='Felhasználónév']")
    login_name.send_keys("a")
    login_password = driver.find_element(By.XPATH, "//input[@placeholder='Jelszó']")
    login_password.send_keys("a")
    login_remember = driver.find_element(By.ID, "rememberMe")
    login_remember.click()
    login_captcha = ""
    try:
        login_captcha = driver.find_element(By.CLASS_NAME, "recaptcha-checkbox-border")
        login_captcha.click()
        login_send = driver.find_element(By.XPATH, "//*[text()='Belépés']")
        login_send.click()
        raise AssertionError("CAPTCHA megállította a botolt bejelentkezést.")
    except NoSuchElementException:
        pass
