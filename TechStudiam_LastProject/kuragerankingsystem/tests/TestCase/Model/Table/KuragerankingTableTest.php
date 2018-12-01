<?php
namespace App\Test\TestCase\Model\Table;

use App\Model\Table\KuragerankingTable;
use Cake\ORM\TableRegistry;
use Cake\TestSuite\TestCase;

/**
 * App\Model\Table\KuragerankingTable Test Case
 */
class KuragerankingTableTest extends TestCase
{

    /**
     * Test subject
     *
     * @var \App\Model\Table\KuragerankingTable
     */
    public $Kurageranking;

    /**
     * Fixtures
     *
     * @var array
     */
    public $fixtures = [
        'app.kurageranking'
    ];

    /**
     * setUp method
     *
     * @return void
     */
    public function setUp()
    {
        parent::setUp();
        $config = TableRegistry::getTableLocator()->exists('Kurageranking') ? [] : ['className' => KuragerankingTable::class];
        $this->Kurageranking = TableRegistry::getTableLocator()->get('Kurageranking', $config);
    }

    /**
     * tearDown method
     *
     * @return void
     */
    public function tearDown()
    {
        unset($this->Kurageranking);

        parent::tearDown();
    }

    /**
     * Test initialize method
     *
     * @return void
     */
    public function testInitialize()
    {
        $this->markTestIncomplete('Not implemented yet.');
    }

    /**
     * Test validationDefault method
     *
     * @return void
     */
    public function testValidationDefault()
    {
        $this->markTestIncomplete('Not implemented yet.');
    }
}
