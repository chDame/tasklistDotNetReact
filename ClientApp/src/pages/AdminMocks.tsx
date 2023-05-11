import React, { useState, useEffect } from 'react';
import adminMockService from '../service/AdminMockService';
import AdminMocksList from '../components/AdminMocksList';
import { useTranslation } from "react-i18next";
import { Tabs, Tab } from 'react-bootstrap';

function AdminMocks() {
  const { t } = useTranslation();
  const [key, setKey] = useState<string>('');

  const loadMocks = async(key:string) => {
	setKey(key);
  }
  
  useEffect(() => {
	loadMocks('clients');
  }, []);

  


  return (
    <Tabs
      id="controlled-tab-example"
      activeKey={key}
      onSelect={(k) => loadMocks(k!)}
      className="mb-3"
    >
      <Tab eventKey="clients" title="Clients">
		{key!='' ?
		<AdminMocksList typeMock="clients"/>
		:<></>}
      </Tab>
      <Tab eventKey="demandes" title="Demandes">
		{key!='' ?
		<AdminMocksList typeMock="demandes"/>
		:<></>}
      </Tab>
    </Tabs>
  );
}

export default AdminMocks;
