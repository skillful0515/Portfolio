<?php
namespace App\Model\Table;

use Cake\ORM\Query;
use Cake\ORM\RulesChecker;
use Cake\ORM\Table;
use Cake\Validation\Validator;

/**
 * Kurageranking Model
 *
 * @method \App\Model\Entity\Kurageranking get($primaryKey, $options = [])
 * @method \App\Model\Entity\Kurageranking newEntity($data = null, array $options = [])
 * @method \App\Model\Entity\Kurageranking[] newEntities(array $data, array $options = [])
 * @method \App\Model\Entity\Kurageranking|bool save(\Cake\Datasource\EntityInterface $entity, $options = [])
 * @method \App\Model\Entity\Kurageranking|bool saveOrFail(\Cake\Datasource\EntityInterface $entity, $options = [])
 * @method \App\Model\Entity\Kurageranking patchEntity(\Cake\Datasource\EntityInterface $entity, array $data, array $options = [])
 * @method \App\Model\Entity\Kurageranking[] patchEntities($entities, array $data, array $options = [])
 * @method \App\Model\Entity\Kurageranking findOrCreate($search, callable $callback = null, $options = [])
 */
class KuragerankingTable extends Table
{

    /**
     * Initialize method
     *
     * @param array $config The configuration for the Table.
     * @return void
     */
    public function initialize(array $config)
    {
        parent::initialize($config);

        $this->setTable('kurageranking');
        $this->setDisplayField('Id');
        $this->setPrimaryKey('Id');
    }

    /**
     * Default validation rules.
     *
     * @param \Cake\Validation\Validator $validator Validator instance.
     * @return \Cake\Validation\Validator
     */
    public function validationDefault(Validator $validator)
    {
        $validator
            ->integer('Id')
            ->allowEmpty('Id', 'create');

        $validator
            ->scalar('Name')
            ->maxLength('Name', 16)
            ->requirePresence('Name', 'create')
            ->notEmpty('Name');

        $validator
            ->integer('Score')
            ->requirePresence('Score', 'create')
            ->notEmpty('Score');

        $validator
            ->dateTime('Date')
            ->requirePresence('Date', 'create')
            ->notEmpty('Date');

        return $validator;
    }
}
