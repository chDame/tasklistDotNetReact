import React, { useState, useEffect } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import adminMockService from '../service/AdminMockService';
import api from '../service/api';
import { useTranslation } from "react-i18next";
import { Alert, Modal, Button, Table, InputGroup, Form, Row, Col } from 'react-bootstrap';
import CodeMirror from '@uiw/react-codemirror';
import { json } from '@codemirror/lang-json';

function AdminMocksList(props:any) {
  const { t } = useTranslation();
  const dispatch = useDispatch();
  const [mock, setMock] = useState("");
  const [mockName, setMockName] = useState("");
  const [newMockModal, setNewMockModal] = useState(false);
  const [mocks, setMocks] = useState<string[]>(props.mocks);
  
  const loadMocks = async () => {
	setMocks(await adminMockService.getMocks(props.typeMock));
  }
  
  useEffect(() => {
	loadMocks();
  }, []);
  
  const loadMock = async (name:string) => {
	setMockName(name);
	setMock(JSON.stringify(await adminMockService.getMock(props.typeMock, name), null, 2));
  }
  
  const deleteMock = (name:string, index:number) => {
	adminMockService.deleteMock(props.typeMock, name);
	mocks.splice(index, 1);
  }
  
  const createNewMock = async () => {
	let defaultStr = '{"someAttribute":"someValue"}';
	setMock(defaultStr);
	await adminMockService.saveMock(props.typeMock, mockName, JSON.parse(defaultStr));
    loadMocks();
	setNewMockModal(false);
  }
  const changeMockValue = React.useCallback((value: string, viewUpdate: any) => {
    setMock(value);
  }, []);

  return (
    <div>
      <br />
      <Button variant="primary" onClick={() => setNewMockModal(true)}><i className="bi bi-plus-square"></i> {t("New mock")}</Button>
   <Row className="maileditor">
      <Col className="card">
        <h5 className="card-title bg-primary text-light">{t("Mock list")}</h5>
        <Table striped bordered hover>
		<thead>
		  <tr>
            <th scope="col">{t("Name")}</th>
            <th scope="col">{t("Actions")}</th>
          </tr>
        </thead>
        <tbody>
          {mocks ? mocks.map((mock: string, index: number) =>
            <tr key={mock} className={mock==mockName?'active':''}>
              <td>{mock}</td>
              <td>
                <Button variant="primary" onClick={() => loadMock(mock)}><i className="bi bi-pencil"></i> {t("Open")}</Button>
                <Button variant="danger" onClick={() => deleteMock(mock, index)}><i className="bi bi-trash"></i> {t("Delete")}</Button>
              </td>
            </tr>)
          : <></>}
		</tbody>
      </Table>
      </Col>
      <Col >
		{mock ?
        <div className="card">
          <span className="card-title bg-primary text-light">Mock content</span>
          <CodeMirror
            value={mock}
            extensions={[json()]}
            onChange={changeMockValue}
          />
                <Button variant="primary" onClick={() => adminMockService.saveMock(props.typeMock, mockName, JSON.parse(mock))}><i className="bi bi-pencil"></i> {t("Save")}</Button>
        </div>
		:<></>
		}
      </Col>

    </Row>
      
	  
      <Modal show={newMockModal} onHide={() => setNewMockModal(false)} >
        <Modal.Header closeButton>
          <Modal.Title>New mock</Modal.Title>
        </Modal.Header>
        <Modal.Body>
          <InputGroup className="mb-3">
            <InputGroup.Text>Mock name</InputGroup.Text>
            <Form.Control placeholder="mock name" value={mockName} onChange={(evt) => setMockName(evt.target.value)} />
          </InputGroup>
        </Modal.Body>
        <Modal.Footer>
          <Button variant="primary" onClick={createNewMock}>Create</Button>
          <Button variant="secondary" onClick={() => setNewMockModal(false)}> Close</Button>
        </Modal.Footer>
      </Modal>
  </div >
  );
}

export default AdminMocksList
