import Address from "../classes/Address";
import IAddress from "../interfaces/IAddress";
import MainPage from "../pages/MainPage";
import PolicyAndPrivacyPage from "../pages/PolicyAndPrivacyPage";
import LoginPage from "../pages/authPages/LoginPage";
import RegistrationPage from "../pages/authPages/RegistrationPage";
import NotFoundPage from "../pages/errorPages/NotFoundPage";

const addresses: IAddress[] = [
  new Address("/", <MainPage />),
  new Address("registration", <RegistrationPage />),
  new Address("/login", <LoginPage />),
  new Address("/policyandprivacy", <PolicyAndPrivacyPage />),
  new Address("*", <NotFoundPage />),
];

export default addresses;
