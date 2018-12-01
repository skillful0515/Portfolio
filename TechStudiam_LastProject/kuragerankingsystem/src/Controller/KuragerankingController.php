<?php
namespace App\Controller;

use App\Controller\AppController;

/**
 * Kurageranking Controller
 *
 * @property \App\Model\Table\KuragerankingTable $Kurageranking
 *
 * @method \App\Model\Entity\Kurageranking[]|\Cake\Datasource\ResultSetInterface paginate($object = null, array $settings = [])
 */
class KuragerankingController extends AppController
{

    /**
     * Index method
     *
     * @return \Cake\Http\Response|void
     */
    public function index()
    {
        $kurageranking = $this->paginate($this->Kurageranking);

        $this->set(compact('kurageranking'));
    }

    /**
     * View method
     *
     * @param string|null $id Kurageranking id.
     * @return \Cake\Http\Response|void
     * @throws \Cake\Datasource\Exception\RecordNotFoundException When record not found.
     */
    public function view($id = null)
    {
        $kurageranking = $this->Kurageranking->get($id, [
            'contain' => []
        ]);

        $this->set('kurageranking', $kurageranking);
    }

    /**
     * Add method
     *
     * @return \Cake\Http\Response|null Redirects on successful add, renders view otherwise.
     */
    public function add()
    {
        $kurageranking = $this->Kurageranking->newEntity();
        if ($this->request->is('post')) {
            $kurageranking = $this->Kurageranking->patchEntity($kurageranking, $this->request->getData());
            if ($this->Kurageranking->save($kurageranking)) {
                $this->Flash->success(__('The kurageranking has been saved.'));

                return $this->redirect(['action' => 'index']);
            }
            $this->Flash->error(__('The kurageranking could not be saved. Please, try again.'));
        }
        $this->set(compact('kurageranking'));
    }

    /**
     * Edit method
     *
     * @param string|null $id Kurageranking id.
     * @return \Cake\Http\Response|null Redirects on successful edit, renders view otherwise.
     * @throws \Cake\Network\Exception\NotFoundException When record not found.
     */
    public function edit($id = null)
    {
        $kurageranking = $this->Kurageranking->get($id, [
            'contain' => []
        ]);
        if ($this->request->is(['patch', 'post', 'put'])) {
            $kurageranking = $this->Kurageranking->patchEntity($kurageranking, $this->request->getData());
            if ($this->Kurageranking->save($kurageranking)) {
                $this->Flash->success(__('The kurageranking has been saved.'));

                return $this->redirect(['action' => 'index']);
            }
            $this->Flash->error(__('The kurageranking could not be saved. Please, try again.'));
        }
        $this->set(compact('kurageranking'));
    }

    /**
     * Delete method
     *
     * @param string|null $id Kurageranking id.
     * @return \Cake\Http\Response|null Redirects to index.
     * @throws \Cake\Datasource\Exception\RecordNotFoundException When record not found.
     */
    public function delete($id = null)
    {
        $this->request->allowMethod(['post', 'delete']);
        $kurageranking = $this->Kurageranking->get($id);
        if ($this->Kurageranking->delete($kurageranking)) {
            $this->Flash->success(__('The kurageranking has been deleted.'));
        } else {
            $this->Flash->error(__('The kurageranking could not be deleted. Please, try again.'));
        }

        return $this->redirect(['action' => 'index']);
    }

    public function getMessage()
    {
        error_log("getMessage()");

        $this->autoRender = false;

        $query = $this->Kurageranking->find('all');

        $query->order(['score' => 'DESC']);

        $json_array = json_encode($query);

        echo $json_array;
    }

    public function setMessage()
    {
        error_log("setMessage()");

        $this->autoRender = false;

        $name = $this->request->data['name'];
        $score = $this->request->data['score'];

        // テーブルに追加するレコード情報を作る
        $data = array('Name' => $name, 'Score' => $score, 'Date' => date('Y/m/d H:i:s'));

        $kurageranking = $this->Kurageranking->newEntity();
        $kurageranking = $this->Kurageranking->patchEntity($kurageranking, $data);
        if($this->Kurageranking->save($kurageranking)){
            echo "1";
        }else{
            echo "0";
        }
    }
}
